using Godot;
using System;

namespace BehaviorTree.Nodes.Leaf.Actions
{
    public class SetTargetToPlayer : SetTarget
    {
        protected override bool SetTargetPosition()
        {
            TargetPosition = Player.PlayerQuickAccess.KINEMATIC_BODY.GlobalTranslation;
            return true;
        }
    }

}
