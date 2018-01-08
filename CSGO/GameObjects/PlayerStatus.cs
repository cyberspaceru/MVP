using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVP.Connectors;

namespace MVP.CSGO.GameObjects
{
    public class PlayerStatus : ProcessReader
    {
        public readonly long PhysicsOffsetAsPointer = 0x10;
        public PlayerPhysics PlayerPhysics { get; private set; }

        public PlayerStatus(MvProcess process, long anchor) : base(process, anchor)
        {
        }

        public override void Read()
        {
            if (PlayerPhysics == null)
            {
                PlayerPhysics = new PlayerPhysics(Process, Process.ReadPointer32(Anchor + PhysicsOffsetAsPointer));
            }
            PlayerPhysics.Read();
        }
    }
}
