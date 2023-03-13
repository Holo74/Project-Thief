using Godot;
using System;

namespace BehaviorTree.Nodes.Leaf.Actions
{
    public partial class SetAnimCondition : Base
    {
        [Export]
        private string AnimPath { get; set; }
        [Export]
        private bool SetConditionTo { get; set; }
        public override Results Tick(double delta, BehaviorController BC)
        {
            BC.AnimTree.Set(AnimPath, SetConditionTo);
            return Results.Success;
        }
    }

}
