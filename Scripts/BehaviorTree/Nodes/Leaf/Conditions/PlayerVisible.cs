using Godot;
using System;

namespace BehaviorTree.Nodes.Leaf.Conditions
{
    public class PlayerVisible : Base
    {
        [Export(PropertyHint.Range, "0, 2")]
        private float Threshold { get; set; }
        public override Results Tick(float delta, BehaviorController BC)
        {
            if (Player.PlayerManager.Instance.GetStealthValue() > Threshold)
            {
                return Results.Success;
            }
            return Results.Failure;
        }
    }

}
