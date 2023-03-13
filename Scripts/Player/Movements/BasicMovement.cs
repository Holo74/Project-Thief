using Godot;
using System;

namespace Player.Movement
{
    public partial class BasicMovement : AbstractMovement
    {
        public override void Movement(double delta)
        {
            if (Input.IsActionJustPressed("ui_select"))
            {
                Jump(Vector3.Zero);
                FallingMovement(delta);
                return;
            }
            Variables.Instance.WALKING_MOVEMENT = DirectionalInput() * MovementSpeed();
            Variables.Instance.MOVEMENT.Crouch();
            Variables.Instance.MOVEMENT.Crawl();
            PlayerQuickAccess.KINEMATIC_BODY.Velocity = TotalMovement();
            PlayerQuickAccess.KINEMATIC_BODY.MoveAndSlide();
        }

        public override void Starting()
        {
            base.Starting();
            PlayerQuickAccess.KINEMATIC_BODY.MotionMode = CharacterBody3D.MotionModeEnum.Grounded;
            PlayerQuickAccess.KINEMATIC_BODY.UpDirection = Vector3.Up;
        }
    }

}
