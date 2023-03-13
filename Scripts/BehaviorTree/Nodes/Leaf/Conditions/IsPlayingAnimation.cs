using Godot;
using System;

namespace BehaviorTree.Nodes.Leaf.Conditions
{
    public partial class IsPlayingAnimation : Base
    {
        [Export]
        private string IsPlayingAnimName { get; set; }
        public override Results Tick(double delta, BehaviorController BC)
        {
            if (GetNode<AnimationPlayer>(BC.AnimTree.AnimPlayer).CurrentAnimation.Equals(IsPlayingAnimName))
            {
                return Results.Success;
            }
            return Results.Failure;
        }
    }

}
