using Godot;
using System;

namespace BehaviorTree.Nodes.Leaf.Conditions
{
    public partial class CurrentTargetIsInRange : Base
    {
        [Export]
        private float Range { get; set; }
        public override Results Tick(double delta, BehaviorController BC)
        {
            return ReturnBoolValue.BoolToResult(BC.NavAgent.DistanceToTarget() <= Range);
        }
    }

}
