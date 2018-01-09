using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVP.Connectors;

namespace MVP.CSGO.GameObjects
{
    public class Entity : Player
    {
        public const int LocalPlayerIndex = 0; // The local player is the first item in the entity list.
        private const long EntityStepSize = 0x10; // Step size between entities in the list.

        public Entity(MvProcess process, long anchor) : base(process, anchor)
        {
        }

        public static Entity ReadAnyByEntityListAnchor(MvProcess process, long entityListAnchor, int index)
        {
            long entityAnchor = process.ReadPointer32(entityListAnchor + index * EntityStepSize); // Read the player pointer.
            return entityAnchor == 0 ? null : new Entity(process, entityAnchor);
        }

        public static Entity ReadLocalPlayerByEntityListAnchor(MvProcess process, long entityListAnchor)
        {
            var entity = ReadAnyByEntityListAnchor(process, entityListAnchor, LocalPlayerIndex);
            entity?.Read();
            return entity;
        }

    }
}
