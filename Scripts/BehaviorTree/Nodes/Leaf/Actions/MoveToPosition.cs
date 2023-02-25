using Godot;
using System;

namespace BehaviorTree.Nodes.Leaf.Actions
{
    public class MoveToPosition : Base
    {
        [Export]
        private float Speed { get; set; }
        public override Results Tick(float delta, BehaviorController BC)
        {
            BC.SetVelocityToPhysics(BC.NavAgent.GetNextLocation().Normalized() * Speed);
            return Results.Success;
        }
    }

}
