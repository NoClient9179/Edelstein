using System;
using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.User;
using Edelstein.Service.Game.Services.Handlers;
public class PassiveskillInfoUpdateHandler : AbstractFieldUserHandler
{
    /* getting an idea of what to send back to the client
    we can encode up to 6 bytes, at least as a response to a 1st job bowman attempting to use focus without any other active or passive skills on
    tLastUpdate<short> 2 bytes
    nBuffId<byte> 1 byte
    tDuration<short> 2 bytes
    StatType<byte> this is the secondary stat?
    State<byte> 1 byte
    secondary stat flags - infinity - regen - dragon blood - poison
    */
    public override async Task Handle(RecvPacketOperations operation, IPacket packet, FieldUser user)
    {
        var p = new Packet();
        Console.WriteLine("UserPassiveskillInfoUpdateHandler");
        //when you put a skill point into a skill, this tells the server to update the char object + db?
        //when you use any skill with passive properties, this is called. for example, use a mount skill and some
        //unique packet is sent so you may identify this and respond accordingly
        _ = packet.Decode<short>(); // opcode
        short idk = packet.Decode<short>() // idk
        byte nBuffId = packet.Decode<byte>(); //probably mislabeled
        //short tDuration = packet.Decode<short>();
        byte State = packet.Decode<byte>(); // probably mislabeled
        //use these values for UserCalcDamageStatSetRequest?
        p.Encode<byte>(0);
        await user.Field.BroadcastPacket(user, p);
    }
}