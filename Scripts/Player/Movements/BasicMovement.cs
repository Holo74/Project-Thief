using Godot;
using System;

namespace Player.Movement
{
    public partial class BasicMovement : AbstractMovement
    {
        public override void Starting()
        {
            base.Starting();
            PlayerQuickAccess.CHARACTER_BODY.UpDirection = Vector3.Up;
        }

        public override void Movement(double delta)
        {
            if (Input.IsActionJustPressed("Jump"))
            {
                Jump(Vector3.Zero);
                return;
            }
            Variables.Instance.WALKING_MOVEMENT = DirectionalInput() * ((float)MovementSpeed());
            Variables.Instance.MOVEMENT.Crouch();
            Variables.Instance.MOVEMENT.Crawl();
            PlayerQuickAccess.CHARACTER_BODY.Velocity = TotalMovement();
            PlayerQuickAccess.CHARACTER_BODY.MoveAndSlide();
        }
    }

}
