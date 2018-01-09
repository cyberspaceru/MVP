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
        public float CameraHeight { get; private set; }
        public Vector3 Velocity { get; private set; }
        public Vector2 Angle { get; private set; }
        public Vector3 Position { get; private set; }
        // We can find the player position and in order to get the camera position without an offset for one, we just apply some trick.
        public Vector3 CameraPosition => new Vector3(Position.x, Position.y + CameraHeight, Position.z);

        public readonly long HealthOffset = 0xFC;
        public readonly long TeamOffset = 0xF0;
        public readonly long DormantOffset = 0xE9;
        public readonly long CameraHeightOffset = 0x10C;
        public readonly long VelocityOffset = 0x110;
        public readonly long AngleOffset = 0x128;
        public readonly long PositionOffset = 0x134;

        protected Player(MvProcess process, long anchor) : base(process, anchor)
        {

        }

        public override void Read()
        {
            Health = Process.ReadInt32(Anchor + HealthOffset);
            Team = Process.ReadInt32(Anchor + TeamOffset);
            Dormant = Process.ReadBool(Anchor + DormantOffset);
            CameraHeight = Process.ReadFloat(Anchor + CameraHeightOffset);
            Velocity = Process.ReadVector3(Anchor + VelocityOffset, ProcessReaders.Vector3StorageType.Xzy);
            Position = Process.ReadVector3(Anchor + PositionOffset, ProcessReaders.Vector3StorageType.Xzy);
            Angle = Process.ReadVector2(Anchor + AngleOffset);
        }

        public override string ToString()
        {
            return $"{nameof(Health)}: {Health}," +
                   $" {nameof(Team)}: {Team}," +
                   $" {nameof(Dormant)}: {Dormant}," +
                   $" {nameof(Angle)}: {Angle}," +
                   $" {nameof(CameraPosition)}: {CameraPosition}";
        }
    }
}
