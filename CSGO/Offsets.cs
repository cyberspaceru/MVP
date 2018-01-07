using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVP.Connectors;
using MVP.GameConcepts;

namespace MVP.CSGO
{
    public static class Offsets
    {
        public const string LocalPlayerPattern = "8D 34 85 ? ? ? ? 89 15";
        public const string EntityListPattern = "42 18 3B C7";
        public const string ViewMatrixPattern = "0F 10 05 ? ? ? ? 8D 85 ? ? ? ? B9";

        public class Player
        {
            public long Base { get; }

            public int Health { get; }
            public int Team { get; }
            public bool Dormant { get; }
            public Vector3 Position { get; }

            public readonly long HealthOffset = 0xFC;
            public readonly long TeamOffset = 0xF0;
            public readonly long DormantOffset = 0xE9;
            public readonly long PositionOffset = 0x134;

            public Player(MvProcess process, long @base)
            {
                this.Base = @base;
                Health = process.ReadInt32(Base + HealthOffset);
                Team = process.ReadInt32(Base + TeamOffset);
                Dormant = process.ReadBool(Base + DormantOffset);
                Position = process.ReadVector3(Base + PositionOffset);
            }

            public override string ToString()
            {
                return $"{nameof(Base)}: {Base}, {nameof(Health)}: {Health}, {nameof(Team)}: {Team}, {nameof(Dormant)}: {Dormant}, {nameof(Position)}: {Position}";
            }
        }

        public class LocalPlayer : Player
        {
            public LocalPlayer(MvProcess process, long @base) : base(process, @base)
            {
            }
        }

        public class AnotherPlayer : Player
        {
            public AnotherPlayer(MvProcess process, long @base) : base(process, @base)
            {
            }
        }

    }
}
