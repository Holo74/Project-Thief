using Godot;
using System;

namespace Player.Movement
{
    public partial class Sliding : AbstractMovement
    {
        private float Modifier { get; set; }
        public Sliding(float modifier)
        {
            Modifier = modifier;
        }

        public override void Starting()
        {
            base.Starting();
            PlayerQuickAccess.CHARACTER_BODY.UpDirection = Vector3.Up;
        }

        public override void Movement(double delta)
        {
            if (Input.IsActionJustPressed("Jump"))
            {
                Jump(Variables.Instance.WALKING_MOVEMENT * Modifier);
                FallingMovement(delta);
                Variables.Instance.RESET_MOVEMENT();
                return;
            }
            PlayerQuickAccess.CHARACTER_BODY.Velocity = TotalMovement() * Modifier;
            PlayerQuickAccess.CHARACTER_BODY.MoveAndSlide();
            if (Variables.Instance.WALKING_MOVEMENT.Length() < Variables.Instance.CROUCH_SPEED)
            {
                Variables.Instance.RESET_MOVEMENT();
            }
            Variables.Instance.WALKING_MOVEMENT *= .99f;
        }
    }

}
