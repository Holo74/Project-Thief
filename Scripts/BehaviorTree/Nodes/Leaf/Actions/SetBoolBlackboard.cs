using Godot;
using System;

namespace BehaviorTree.Nodes.Leaf.Actions
{
    public partial class SetBoolBlackboard : Base
    {
        [Export]
        private Enums.KeyList KeyToChange { get; set; }
        [Export]
        private bool ValueToSet { get; set; }

        public override Results Tick(double delta, BehaviorController BC)
        {
            base.Tick(delta, BC);
            BC.BlackBoard[KeyToChange] = ValueToSet;
            BC.BlackBoard[Enums.KeyList.Debugging] = "Setting " + KeyToChange.ToString() + " To the bool value " + ValueToSet;
            return Results.Success;
        }
    }
}

