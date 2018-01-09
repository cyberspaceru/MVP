using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVP.Connectors;

namespace MVP.CSGO.GameObjects
{
    public abstract class ProcessReader
    {
        public MvProcess Process { get; }
        public long Anchor { get; } // Don't storage pointers (Anchor is the base address which using to initializing an object with offsets).

        protected ProcessReader(MvProcess process, long anchor)
        {
            Process = process;
            Anchor = anchor;
        }

        public abstract void Read();
    }
}
