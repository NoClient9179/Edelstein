using Edelstein.Network.Packets;
using Edelstein.Network.Transport;

namespace Edelstein.Service.Game.Helpers
{
        public sealed class TagPoint
        {
            public short X { get; set; }
            public short Y { get; set; }

            public TagPoint() { }

            public TagPoint(short x, short y)
            {
            //these are shorts motherfucker dont forget
                X = x;
                Y = y;
            }

            public void Encode(IPacket p)
            {
                p.Encode(X);
                p.Encode(Y);
            }

            public TagPoint Clone() => new TagPoint(X, Y);
            public override string ToString() => $"[X {X}] [Y {Y}]";
        }
    }
