using Godot;
using System;

namespace Player.Upgrades
{
    public partial class DoubleJump : AbstractUpgrade
    {
        private int JumpAmount { get; set; } = 0;
        private int frameBuffer = 1;
        public override void Applied()
        {
            Variables.Instance.OnFloorChange += FloorChange;
        }

        public override void Update(double delta)
        {
            if (!Player.Variables.Instance.ON_FLOOR)
            {
                // This way you don't jump again when you first jump
                if (frameBuffer == 1)
                {
                    frameBuffer = 0;
                    // Skips the first frame
                    return;
                }
                if (Input.IsActionJustPressed("Jump") && JumpAmount < 1)
                {
                    JumpAmount += 1;
                    Player.Variables.Instance.GRAVITY_MOVEMENT = Vector3.Up * ((float)Player.Variables.Instance.JUMP_STRENGTH);
                }
            }

        }

        public override void Removed()
        {
            Variables.Instance.OnFloorChange -= FloorChange;
        }

        private void FloorChange(bool floor)
        {
            // Resets time buffer when you land
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
