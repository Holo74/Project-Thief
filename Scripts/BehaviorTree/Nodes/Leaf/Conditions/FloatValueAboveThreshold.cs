using Godot;
using System;

namespace BehaviorTree.Nodes.Leaf.Conditions
{
    public partial class FloatValueAboveThreshold : Base
    {
        [Export]
        private float Threshold { get; set; }
        [Export]
        private BehaviorTree.Enums.KeyList Key { get; set; }
        public override Results Tick(double delta, BehaviorController BC)
        {
            if (BC.BlackBoard.ContainsKey(Key))
            {
                if ((float)BC.BlackBoard[Key] > Threshold)
                {
                    return Results.Success;
                }
            }
            return Results.Failure;
        }
    }

}
