using Godot;
using System;

namespace BehaviorTree.Nodes.Leaf.Conditions
{
    public partial class ReturnBoolValue : Base
    {
        [Export]
        private BehaviorTree.Enums.KeyList Key { get; set; }
        [Export]
        private bool ExpectedVal { get; set; }
        [Export]
        private bool DefaultVal { get; set; }
        public override Results Tick(double delta, BehaviorController BC)
        {
            base.Tick(delta, BC);
            if (BC.BlackBoard.ContainsKey(Key))
            {
                return BoolToResult((BC.BlackBoard[Key].AsBool()) == ExpectedVal);
            }
            BC.BlackBoard[Key] = DefaultVal;
            return Results.Failure;
        }

        public static Results BoolToResult(bool b)
        {
            if (b)
            {
                return Results.Success;
            }
            return Results.Failure;
        }
    }

}
