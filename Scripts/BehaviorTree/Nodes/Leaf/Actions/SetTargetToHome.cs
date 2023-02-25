using Godot;
using System;

namespace BehaviorTree.Nodes.Leaf.Actions
{
    public class SetTargetToHome : SetTarget
    {
        [Export]
        private Vector3 GlobalPosition;
        protected override bool SetTargetPosition()
        {
            TargetPosition = GlobalPosition;
            return true;
        }
    }

}
