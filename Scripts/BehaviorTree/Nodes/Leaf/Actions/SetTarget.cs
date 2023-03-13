using Godot;
using System;

namespace BehaviorTree.Nodes.Leaf.Actions
{
    public abstract partial class SetTarget : Base
    {
        protected Vector3 TargetPosition { get; set; }
        public override Results Tick(double delta, BehaviorController BC)
        {
            if (SetTargetPosition())
            {
                BC.NavAgent.TargetPosition = (TargetPosition);
                return Results.Success;
            }
            return Results.Failure;
        }

        protected abstract bool SetTargetPosition();
    }
}

