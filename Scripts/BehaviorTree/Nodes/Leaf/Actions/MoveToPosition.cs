using Godot;
using System;

namespace BehaviorTree.Nodes.Leaf.Actions
{
    public partial class MoveToPosition : Base
    {
        [Export]
        private float Speed { get; set; }
        public override Results Tick(double delta, BehaviorController BC)
        {
            BC.SetVelocityToPhysics((BC.NavAgent.GetNextPathPosition() - BC.GlobalPosition).Normalized() * Speed);
            return Results.Success;
        }
    }

}
