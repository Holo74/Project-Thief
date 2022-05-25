using Godot;
using System;

namespace Environment.Areas
{
    public class WaterEnterance : PlayerArea
    {
        protected override void PlayerEntered()
        {
            Player.Variables.MOVEMENT = new Player.Movement.Swimming();
        }

        protected override void PlayerLeft()
        {
            if (Player.PlayerQuickAccess.KINEMATIC_BODY.GlobalTransform.origin.y > GlobalTransform.origin.y)
            {
                if (Player.Variables.MOVEMENT is Player.Movement.Swimming)
                {
                    Player.Variables.RESET_MOVEMENT();
                }
            }
            else
            {
                if (!(Player.Variables.MOVEMENT is Player.Movement.Swimming))
                {
                    Player.Variables.MOVEMENT = new Player.Movement.Swimming();
                }
            }
        }
    }

}
