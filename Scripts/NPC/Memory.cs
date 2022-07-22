using Godot;
using System;

namespace NPC
{
    public class Memory
    {
        public Memory(Vector3 position, MemoryType type)
        {
            Resolved = false;
            Position = position;
            Type = type;
        }
        public Vector3 Position { get; private set; }
        public bool Resolved { get; private set; }
        public MemoryType Type { get; private set; }

        public void Resolve()
        {
            Resolved = true;
        }
    }

    public enum MemoryType
    {
        Visual,
        Audio,
        Environmental
    }
}
