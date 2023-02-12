using Godot;
using System;

namespace BehaviorTree.Nodes.Leaf.Conditions
{
    public class CurrentTargetIsInRange : Base
    {
        [Export]
        private float Range { get; set; }
        public override Results Tick(float delta, BehaviorController BC)
        {
            return ReturnBoolValue.BoolToResult(BC.NavAgent.DistanceToTarget() <= Range);
        }
    }

}
