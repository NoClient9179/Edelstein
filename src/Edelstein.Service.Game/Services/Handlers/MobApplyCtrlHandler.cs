using System.Threading.Tasks;
using System;
using Edelstein.Core;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.User;
using Edelstein.Service.Game.Services.Handlers;
using Edelstein.Service.Game.Fields.Objects;

namespace Edelstein.Service.Game.Services.Handlers
{
	public class MobApplyCtrlHandler : AbstractFieldUserHandler
	{

        public override async Task Handle(RecvPacketOperations operation, IPacket packet, FieldUser user)
        {
			//Console.WriteLine("MobApplyCtrlHandler");
			//var p = new Packet(SendPacketOperations.UserHit);
			if (user.Character.HP <= 0) return;
			var dwMobID = packet.Decode<int>();
			var nCtrlPriority = packet.Decode<short>();
			using var p = new Packet(SendPacketOperations.MobChangeController);
			p.Encode<bool>(true);
			p.Encode<int>(user.Character.ID);
			await user.Field.BroadcastPacket(user, p);
		}
    }
}
