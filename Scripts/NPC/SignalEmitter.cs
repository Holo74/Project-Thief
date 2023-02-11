using Godot;
using System;

namespace NPC
{
    public class SignalEmitter : Spatial
    {
        [Signal]
        public delegate void CreateMemory(Vector3 position, Node source, MemoryType type);

        [Signal]
        public delegate void SwitchState(NPCManager.BehaviourState state);
        [Signal]
        public delegate void SeenCounter(int changeBy);
    }

}
