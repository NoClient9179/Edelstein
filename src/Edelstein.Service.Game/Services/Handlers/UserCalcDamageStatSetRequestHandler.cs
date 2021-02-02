using System;
using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.User;
using Edelstein.Service.Game.Services.Handlers;
public class UserCalcDamageStatSetRequestHandler : AbstractFieldUserHandler
{
    public override async Task Handle(RecvPacketOperations operation, IPacket packet, FieldUser user)
    {
        //Console.WriteLine("UserCalcDamageStatSetRequestHandler");
        //Console.WriteLine(packet.Buffer.Length);
        //0x78 ?? I am literally just getting an opcode, which is a short? guess nType is something else then?
        //this is requested after PassiveskillInfoUpdate
        var p = new Packet();
        p.Encode<byte>(1);
        await user.Field.BroadcastPacket(user, p);
    }
}