using System;
using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.User;
using Edelstein.Service.Game.Services.Handlers;
public class PassiveskillInfoUpdateHandler : AbstractFieldUserHandler
{
    /* getting an idea of what to send back to the client
     * we can encode up to 6 bytes, at least as a response to a 1st job bowman attempting to use focus without any other active or passive skills on
    tLastUpdate<short> 2 bytes
    nBuffId<byte> 1 byte
    tDuration<short> 2 bytes
    StatType<byte> this is the secondary stat?
    State<byte> 1 byte
    secondary stat flags - infinity - regen - dragon blood - poison
    */
    public override async Task Handle(RecvPacketOperations operation, IPacket packet, FieldUser user)
    {
        var p = new Packet(SendPacketOperations.PassiveskillInfoUpdate);
        Console.WriteLine("UserPassiveskillInfoUpdateHandler");
        short tLastUpdate = packet.Decode<short>();
        byte nBuffId = packet.Decode<byte>();
        short tDuration = packet.Decode<short>();
        byte State = packet.Decode<byte>();
        //p.Encode<short>(tLastUpdate);
        //p.Encode<byte>(nBuffId);
        //p.Encode<short>(tDuration);
        //p.Encode<byte>(State);
        p.Encode<byte>(0);
        await user.Field.BroadcastPacket(user, p);
    }
}