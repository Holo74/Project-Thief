using Godot;
using System;

namespace BehaviorTree.Nodes.Leaf.Conditions
{
    public class FloatValueAboveThreshold : Base
    {
        [Export]
        private float Threshold { get; set; }
        [Export]
        private String Key { get; set; }
        public override Results Tick(float delta, BehaviorController BC)
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