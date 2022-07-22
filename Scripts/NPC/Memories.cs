using Godot;
using System;

namespace NPC
{
    public class Memories
    {
        private System.Collections.Generic.Queue<Memory> MemoryStorage { get; set; }
        public Memory CurrentMemory { get; private set; }

        public void ResolveCurrentMemory()
        {
            if (CurrentMemory != null)
            {
                CurrentMemory.Resolve();
            }
            if (MemoryStorage.Count > 0)
            {
                CurrentMemory = MemoryStorage.Dequeue();
            }
        }

        public bool IsMemoryResolved()
        {
            if (CurrentMemory != null)
            {
                return CurrentMemory.Resolved;
            }
            return true;
        }

        public void CreateMemory(MemoryType type, Vector3 position)
        {
            Memory holder = new Memory(position, type);
            MemoryStorage.Enqueue(holder);
            if (MemoryStorage.Count > 4)
            {
                MemoryStorage.Dequeue();
            }
        }
    }
}

