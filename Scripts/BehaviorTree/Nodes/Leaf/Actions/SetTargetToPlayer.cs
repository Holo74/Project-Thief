using Godot;
using System;

namespace BehaviorTree.Nodes.Leaf.Actions
{
    public partial class SetTargetToPlayer : SetTarget
    {
        protected override bool SetTargetPosition()
        {
            TargetPosition = Player.PlayerQuickAccess.CHARACTER_BODY.GlobalPosition;
            return true;
        }
    }

}
