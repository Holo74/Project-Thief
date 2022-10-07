using Godot;
using System;

namespace NPC
{
    public class Memories
    {
        public Memories()
        {
            MemoryStorage = new System.Collections.Generic.Queue<Memory>();
        }

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

        public void CreateMemory(MemoryType type, Vector3 position, Node source)
        {
            Memory holder = new Memory(position, type, source);
            bool hasMemory = false;
            if (CurrentMemory is null)
            {
                CurrentMemory = holder;
            }
            foreach (Memory m in MemoryStorage)
            {
                if (m.SimilarMemory(holder))
                {
                    hasMemory = true;
                    m.UpdateMemory(position);
                    break;
                }
            }
            if (!hasMemory)
            {
                MemoryStorage.Enqueue(holder);
            }

            if (MemoryStorage.Count > 4)
            {
                CurrentMemory = MemoryStorage.Dequeue();
            }
        }
    }
}

