using Godot;
using System;

namespace BehaviorTree.Nodes.Leaf.Actions
{
    public abstract class SetTarget : Base
    {
        protected Vector3 TargetPosition { get; set; }
        public override Results Tick(float delta, BehaviorController BC)
        {
            if (SetTargetPosition())
            {
                BC.NavAgent.SetTargetLocation(TargetPosition);
                return Results.Success;
            }
            return Results.Failure;
        }

        protected abstract bool SetTargetPosition();
    }
}

