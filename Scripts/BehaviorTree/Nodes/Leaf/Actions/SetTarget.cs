using Godot;
using System;

namespace BehaviorTree.Nodes.Leaf.Actions
{
    public abstract partial class SetTarget : Base
    {
        protected Vector3 TargetPosition { get; set; }
        protected string Target { get; set; }
        public override Results Tick(double delta, BehaviorController BC)
        {
            base.Tick(delta, BC);
            // BC.BlackBoard[Enums.KeyList.Debugging] = "Setting target to " + Target;
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

