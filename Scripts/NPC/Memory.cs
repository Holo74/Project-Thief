using Godot;
using System;

namespace NPC
{
    public class Memory
    {
        public Memory(Vector3 position, MemoryType type, Node source)
        {
            Resolved = false;
            Position = position;
            Type = type;
            Source = source;
        }
        private Node Source { get; set; }
        public Vector3 Position { get; private set; }
        public bool Resolved { get; private set; }
        public MemoryType Type { get; private set; }

        public void UpdateMemory(Vector3 position)
        {
            Position = position;
        }

        public bool SimilarMemory(Memory m)
        {
            return m.Type == Type && Source.Equals(m.Source);
        }


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
