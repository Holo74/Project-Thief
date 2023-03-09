using Godot;
using System;

namespace Environment.Areas
{
    public partial class WaterEnterance : PlayerArea
    {
        protected override void PlayerEntered()
        {
            Player.Variables.Instance.MOVEMENT = new Player.Movement.Swimming();
        }

        protected override void PlayerLeft()
        {
            if (Player.PlayerQuickAccess.CHARACTER_BODY.GlobalPosition.Y > GlobalPosition.Y)
            {
                if (Player.Variables.Instance.MOVEMENT is Player.Movement.Swimming)
                {
                    Player.Variables.Instance.RESET_MOVEMENT();
                }
            }
            else
            {
                if (!(Player.Variables.Instance.MOVEMENT is Player.Movement.Swimming))
                {
                    Player.Variables.Instance.MOVEMENT = new Player.Movement.Swimming();
                }
            }
        }
    }

}
