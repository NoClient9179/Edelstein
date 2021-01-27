using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.User;
using Edelstein.Service.Game.Services.Handlers;
    public class UserHitHandler : AbstractFieldUserHandler
    {
        public override async Task Handle(RecvPacketOperations operation, IPacket packet, FieldUser user)
        {
            //int dwCharId, byte nAttackIdx, int nDamageRaw, int dwTemplateID,
            //bool bLeft, byte nReflect, bool bGuard,
            //byte nStanceFlag, int nShadowSifterID, int nDamageShownToRemote
            //byte/char = 1, short = 2, int/long = 4, long long = 8

            //Console.WriteLine("UserHitHandler");

            var p = new Packet(SendPacketOperations.UserHit);

            //lets extract the data from this packet (beginner getting hit by a snail)
            int dwCharId = packet.Decode<int>();
            byte nAttackIdx = packet.Decode<byte>();
            int nDamageRaw = packet.Decode<int>();
            int dwTemplateID = packet.Decode<int>();
            bool bLeft = packet.Decode<bool>();
            byte nReflect = packet.Decode<byte>();
            bool bGuard = packet.Decode<bool>();
            byte nStanceFlag = packet.Decode<byte>();
            int nDamageShownToRemote = packet.Decode<int>();
            short newShort = (short)(nDamageRaw / 256);

            //lets use this data to create a packet to send back
            p.Encode<int>(dwCharId);
            p.Encode<byte>(nAttackIdx);
            p.Encode<int>(nDamageRaw);
            p.Encode<int>(dwTemplateID);
            p.Encode<bool>(bLeft);
            p.Encode<byte>(nReflect);
            p.Encode<bool>(bGuard);
            p.Encode<byte>(nStanceFlag);
            p.Encode<int>(nDamageShownToRemote);

            //now, let's respond to the client's request with this response packet
            await user.Field.BroadcastPacket(user, p);
            await user.ModifyStats(s =>
            {
            s.HP -= newShort;
            s.MP += 0;
            });

    }
    }