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
            base.Tick(delta, BC);
            BC.AnimTree.Set(AnimPath, SetConditionTo);
            BC.BlackBoard[Enums.KeyList.Debugging] = "Setting animation to " + AnimPath + " With the value of " + SetConditionTo;
            return Results.Success;
        }
    }

}
