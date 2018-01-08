using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVP.Connectors;
using MVP.GameConcepts;

namespace MVP.CSGO.GameObjects
{
    public class PlayerPhysics : ProcessReader
    {
        public readonly long PositionOffset = 0x30;
        public readonly long VelocityOffset = 0xC;

        public Vector3 Position { get; private set; }
        public Vector3 Velocity { get; private set; }

        public PlayerPhysics(MvProcess process, long anchor) : base(process, anchor)
        {
        }

        public override void Read()
        {
            Position = Process.ReadVector3(Anchor + PositionOffset, ProcessReaders.Vector3StorageType.Xzy);
            Velocity = Process.ReadVector3(Anchor + VelocityOffset, ProcessReaders.Vector3StorageType.Xzy);
        }

        public override string ToString()
        {
            return $"{nameof(Position)}: {Position}";
        }
    }
}
