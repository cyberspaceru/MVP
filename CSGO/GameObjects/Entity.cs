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
        public const int LocalPlayerIndex = 0;
        private const long EntityStepSize = 0x10; // Размер между сущностями в списки.

        public Entity(MvProcess process, long anchor) : base(process, anchor)
        {
        }

        public static Entity ReadAnyByEntityListAnchor(MvProcess process, long anchor, int entityIndex)
        {
            long entityAnchor = process.ReadPointer32(anchor + entityIndex * EntityStepSize); // Читаем указатель на игрока.
            return entityAnchor == 0 ? null : new Entity(process, entityAnchor);
        }

        public static Entity ReadLocalPlayerByEntityListAnchor(MvProcess process, long anchor)
        {
            var entity = ReadAnyByEntityListAnchor(process, anchor, LocalPlayerIndex);
            entity?.Read();
            return entity;
        }

    }
}
