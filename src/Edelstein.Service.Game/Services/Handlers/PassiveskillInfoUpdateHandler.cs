using System;
using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.User;
using Edelstein.Service.Game.Services.Handlers;
public class PassiveskillInfoUpdateHandler : AbstractFieldUserHandler
{
    /* getting an idea of what to send back to the client
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
        //when you put a skill point into a skill, this tells the server to do something here
        //when you use any skill with passive properties, this is called. for example, use a mount/buff skill and some
        //unique packet is sent so you may identify this and respond accordingly
        //i should probably be calling some buff functions in the user character object if they're there
        //in rebirth, aura flag makes us call a unique AddAura function
        _ = packet.Decode<short>(); // opcode
        short idk = packet.Decode<short>() // idk, tLastUpdate?
        var nBuffId = packet.Decode<byte>(); //probably mislabeled
        //short tDuration = packet.Decode<short>();
        byte State = packet.Decode<byte>(); // probably mislabeled
        //use these values for UserCalcDamageStatSetRequest?
        p.Encode<byte>(0);
        await user.Field.BroadcastPacket(user, p);
    }
}