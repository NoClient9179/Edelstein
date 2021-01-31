using System.Threading.Tasks;
using System;
using Edelstein.Core;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.User;
using Edelstein.Service.Game.Services.Handlers;
    public class UserHitHandler : AbstractFieldUserHandler
    {
        public override async Task Handle(RecvPacketOperations operation, IPacket packet, FieldUser user)
    {
        //from getting hit by a snail
        //this is without any active or passive skills.
        Console.WriteLine("UserHitHandler");
        if (packet.Buffer.Length > 25 || packet.Buffer.Length < 25) { Console.Write("not handled? buffer length: " + packet.Buffer.Length); }
        var p = new Packet(SendPacketOperations.UserHit); //this is equal to v1 in the client, which is always supposed to be the packet?
        //opcodes are ints, called nType.
        _ = packet.Decode<int>(); // timestamp + opcode? sometimes other things like tLastUpdate?
        byte nReflect = packet.Decode<byte>(); // stupid number that is always wrong
        byte bGuard = packet.Decode<byte>();
        int nDamageInternal = packet.Decode<int>();
        short dwMobTemplateID = packet.Decode<short>();
        int nDamager = packet.Decode<int>();
        byte bLeft = packet.Decode<byte>();
        byte nKnockBack = packet.Decode<byte>();
        byte nStanceFlag = p.Decode<byte>();
        //game logic
        await user.ModifyStats(s =>
        {
            s.HP -= nDamageInternal;
            //s.MP += 0; // add magic guard logic
        });
    }
}