using Godot;
using System;

namespace Player.Upgrades
{
    public class DoubleJump : AbstractUpgrade
    {
        private int JumpAmount { get; set; } = 0;
        private int frameBuffer = 1;
        public override void Applied()
        {
            Variables.OnFloorChange += FloorChange;
        }

        public override void Update(float delta)
        {
            if (!Player.Variables.ON_FLOOR)
            {
                if (frameBuffer == 1)
                {
                    frameBuffer = 0;
                    return;
                }
                if (Input.IsActionJustPressed("ui_select") && JumpAmount < 1)
                {
                    JumpAmount += 1;
                    Player.Variables.GRAVITY_MOVEMENT = Vector3.Up * Player.Variables.JUMP_STRENGTH;
                }
            }

        }

        public override void Removed()
        {
            Variables.OnFloorChange -= FloorChange;
        }

        private void FloorChange(bool floor)
        {
            if (floor)
            {
                JumpAmount = 0;
                frameBuffer = 1;
            }
            else
            {

            }
        }
    }

}
