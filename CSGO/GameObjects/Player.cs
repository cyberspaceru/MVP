using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVP.Connectors;
using MVP.GameConcepts;

namespace MVP.CSGO.GameObjects
{
    public abstract class Player : ProcessReader
    {
        public int Health { get; private set; }
        public int Team { get; private set; }
        public bool Dormant { get; private set; }
        public Vector3 Position { get; private set; }
        public PlayerStatus PlayerStatus { get; private set; }

        public readonly long HealthOffset = 0xFC;
        public readonly long TeamOffset = 0xF0;
        public readonly long DormantOffset = 0xE9;
        public readonly long PositionOffset = 0x134;

        public readonly long PlayerStatusOffsetAsPointer = 0x24;

        protected Player(MvProcess process, long anchor) : base(process, anchor)
        {

        }

        public override void Read()
        {
            Health = Process.ReadInt32(Anchor + HealthOffset);
            Team = Process.ReadInt32(Anchor + TeamOffset);
            Dormant = Process.ReadBool(Anchor + DormantOffset);
            Position = Process.ReadVector3(Anchor + PositionOffset, ProcessReaders.Vector3StorageType.Xzy);
            if (PlayerStatus == null)
            {
                PlayerStatus = new PlayerStatus(Process, Process.ReadPointer32(Anchor + PlayerStatusOffsetAsPointer));
            }
            PlayerStatus.Read();
        }

        public override string ToString()
        {
            return $"{nameof(Anchor)}: {Anchor:X}, {nameof(Health)}: {Health}, {nameof(Team)}: {Team}, {nameof(Dormant)}: {Dormant}, {nameof(Position)}: {Position}";
        }
    }
}
