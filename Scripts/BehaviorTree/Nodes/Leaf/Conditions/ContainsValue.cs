using Godot;
using System;

namespace BehaviorTree.Nodes.Leaf.Conditions
{
    public class ContainsValue : Base
    {
        [Export]
        private Enums.KeyList Value { get; set; }
        public override Results Tick(float delta, BehaviorController BC)
        {
            if (BC.BlackBoard.ContainsKey(Value))
            {
                return Results.Success;
            }
            return Results.Failure;
        }
    }

}
