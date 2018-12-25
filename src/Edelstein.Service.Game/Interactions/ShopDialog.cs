using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Constants;
using Edelstein.Core.Extensions;
using Edelstein.Core.Services;
using Edelstein.Data.Entities.Inventory;
using Edelstein.Network.Packet;
using Edelstein.Provider.Templates.Item;
using Edelstein.Provider.Templates.Server.NPCShop;
using Edelstein.Service.Game.Fields.User;

namespace Edelstein.Service.Game.Interactions
{
    public class ShopDialog : IDialog
    {
        private readonly FieldUser _user;
        private readonly NPCShopTemplate _template;

        public ShopDialog(FieldUser user, NPCShopTemplate template)
        {
            _user = user;
            _template = template;
        }

        public Task OnPacket(RecvPacketOperations operation, IPacket packet)
        {
            var type = (ShopRequest) packet.Decode<byte>();

            switch (type)
            {
                case ShopRequest.Buy:
                    return OnShopBuyRequest(packet);
                case ShopRequest.Sell:
                    return OnShopSellRequest(packet);
                case ShopRequest.Recharge:
                    return OnShopRechargeRequest(packet);
                case ShopRequest.Close:
                    return _user.Interact(this, true);
            }

            return Task.CompletedTask;
        }

        private async Task OnShopBuyRequest(IPacket packet)
        {
            var pos = packet.Decode<short>();
            var templateID = packet.Decode<int>();
            var count = packet.Decode<short>();
            var shopItem = _template.Items.Values
                .Where(i => i.Price > 0 || i.TokenPrice > 0)
                .OrderBy(i => i.ID)
                .ToList()[pos];

            using (var p = new Packet(SendPacketOperations.ShopResult))
            {
                var result = ShopResult.BuySuccess;

                if (shopItem != null)
                {
                    if (shopItem.TemplateID != templateID) result = ShopResult.CantBuyAnymore;
                    if (shopItem.Quantity > 1) count = 1;
                    if (count > shopItem.MaxPerSlot) count = shopItem.MaxPerSlot;
                    if (shopItem.Price > 0)
                        if (_user.Character.Money < shopItem.Price * count)
                            result = ShopResult.BuyNoMoney;
                    if (shopItem.TokenTemplateID > 0)
                        if (_user.Character.GetItemCount(shopItem.TokenTemplateID) <
                            shopItem.TokenPrice * count)
                            result = ShopResult.BuyNoToken;
                    if (shopItem.Stock == 0) result = ShopResult.BuyNoStock;
                    if (_user.Character.Level < shopItem.LevelLimited) result = ShopResult.LimitLevel_Less;

                    var templates = _user.Socket.WvsGame.TemplateManager;
                    var item = templates.Get<ItemTemplate>(shopItem.TemplateID).ToItemSlot();

                    if (item is ItemSlotBundle bundle)
                        if (ItemConstants.IsRechargeableItem(item.TemplateID))
                            bundle.Number = bundle.MaxNumber;
                        else
                            bundle.Number = (short) (count * shopItem.Quantity);
                    if (!_user.Character.HasSlotFor(item)) result = ShopResult.BuyUnknown;

                    if (result == ShopResult.BuySuccess)
                    {
                        if (shopItem.Price > 0)
                            await _user.ModifyStats(s => s.Money -= shopItem.Price * count);
                        if (shopItem.TokenTemplateID > 0)
                            await _user.ModifyInventory(i => i.Remove(
                                shopItem.TokenTemplateID,
                                shopItem.TokenPrice * count
                            ));
                        // TODO: stock stuff
                        //if (shopItem.Stock > 0) shopItem.Stock--;
                        if (shopItem.ItemPeriod > 0)
                            item.DateExpire = DateTime.Now.AddMinutes(shopItem.ItemPeriod);

                        await _user.ModifyInventory(i => i.Add(item));
                    }
                }
                else result = ShopResult.CantBuyAnymore;

                p.Encode<byte>((byte) result);
                await _user.SendPacket(p);
            }
        }

        private async Task OnShopSellRequest(IPacket packet)
        {
            var pos = packet.Decode<short>();
            var templateID = packet.Decode<int>();
            var count = packet.Decode<short>();
            var inventory = _user.Character.GetInventory((ItemInventoryType) (templateID / 1000000));
            var item = inventory.Items.FirstOrDefault(i => i.Position == pos);

            using (var p = new Packet(SendPacketOperations.ShopResult))
            {
                var result = ShopResult.SellSuccess;

                if (item != null)
                {
                    await _user.ModifyInventory(i => i.Remove(item, count));

                    var templates = _user.Socket.WvsGame.TemplateManager;
                    var template = templates.Get<ItemTemplate>(item.TemplateID);

                    var price = template.SellPrice;

                    if (ItemConstants.IsRechargeableItem(item.TemplateID))
                        price += (int) ((item as ItemSlotBundle)?.Number *
                                        (template as ItemBundleTemplate)?.UnitPrice ?? 0);
                    else price *= count;

                    await _user.ModifyStats(s => s.Money += price);
                }
                else result = ShopResult.SellUnkonwn;

                p.Encode<byte>((byte) result);
                await _user.SendPacket(p);
            }
        }

        private async Task OnShopRechargeRequest(IPacket packet)
        {
            var pos = packet.Decode<short>();
            var inventory = _user.Character.GetInventory(ItemInventoryType.Use);
            var item = inventory.Items.FirstOrDefault(i => i.Position == pos);
            var shopItem = _template.Items.Values
                .Where(i => i.Price <= 0 && i.TokenPrice <= 0)
                .OrderBy(i => i.ID)
                .FirstOrDefault(i => i.TemplateID == item?.TemplateID);

            using (var p = new Packet(SendPacketOperations.ShopResult))
            {
                var result = ShopResult.RechargeUnknown;

                if (shopItem != null)
                {
                    if (item is ItemSlotBundle bundle)
                    {
                        var templates = _user.Socket.WvsGame.TemplateManager;
                        var template = templates.Get<ItemTemplate>(item.TemplateID);
                        var count = bundle.MaxNumber - bundle.Number;
                        var price = (int) (
                            (template as ItemBundleTemplate)?.UnitPrice *
                            count ?? 0
                        );

                        result = ShopResult.RechargeSuccess;

                        if (price > 0)
                            if (_user.Character.Money < price)
                                result = ShopResult.RechargeNoMoney;
                        if (count <= 0) result = ShopResult.RechargeUnknown;

                        if (result == ShopResult.RechargeSuccess)
                        {
                            bundle.Number = bundle.MaxNumber;
                            await _user.ModifyStats(s => s.Money -= price);
                            await _user.ModifyInventory(i => i.UpdateQuantity(bundle));
                        }
                    }
                }
                else result = ShopResult.RechargeIncorrectRequest;

                p.Encode<byte>((byte) result);
                await _user.SendPacket(p);
            }
        }

        public IPacket GetStartDialoguePacket()
        {
            using (var p = new Packet(SendPacketOperations.OpenShopDlg))
            {
                p.Encode<int>(_template.ID);

                var items = _template.Items.Values
                    .OrderBy(i => i.ID)
                    .ToList();

                p.Encode<short>((short) items.Count);
                items.ForEach(i =>
                {
                    p.Encode<int>(i.TemplateID);
                    p.Encode<int>(i.Price);
                    p.Encode<byte>(i.DiscountRate);
                    p.Encode<int>(i.TokenTemplateID);
                    p.Encode<int>(i.TokenPrice);
                    p.Encode<int>(i.ItemPeriod);
                    p.Encode<int>(i.LevelLimited);

                    if (!ItemConstants.IsRechargeableItem(i.TemplateID)) p.Encode<short>(i.Quantity);
                    else p.Encode<double>(i.UnitPrice);

                    p.Encode<short>(i.MaxPerSlot);
                });
                return p;
            }
        }
    }
}