using Godot;
using System;

namespace BehaviorTree.Nodes.Flow
{
    public partial class RandomSelection : Base
    {

        public override Results Tick(double delta, BehaviorController BC)
        {
            base.Tick(delta, BC);
            return Children[Management.Game.GameManager.Instance.Generator.RandiRange(0, Children.Count - 1)].Tick(delta, BC);
        }
    }

}
