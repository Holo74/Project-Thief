using Godot;
using System;

namespace NPC.Behaviour
{
    public abstract class BehaviourBase : Resource
    {
        protected NPCManager Manager { get; set; }

        public virtual void Init(NPCManager manager) { Manager = manager; }

        public virtual void StartingNode() { }
        public abstract void UpdatingNode(float delta);
        public abstract void UpdatingPhysicsNode(float delta);
        public abstract void ExitNode();

    }
}

