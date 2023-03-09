using Godot;
using System;

namespace BehaviorTree.Nodes.Leaf.Conditions
{
    public partial class PlayerVisible : Base
    {
        [Export(PropertyHint.Range, "0, 2")]
        private float Threshold { get; set; }
        public override Results Tick(double delta, BehaviorController BC)
        {
            if (Player.PlayerManager.Instance.GetStealthValue() > Threshold)
            {
                return Results.Success;
            }
            return Results.Failure;
        }
    }

}
