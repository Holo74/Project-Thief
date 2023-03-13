using Godot;
using System;

namespace Player.Movement
{
    // This needs a lot of work.  I wouldn't recommeend using it for now
    public partial class Swimming : AbstractMovement
    {
        public override void FallingMovement(double delta)
        {
            Variables.Instance.WALKING_MOVEMENT = DirectionMovement() * Variables.Instance.STANDING_SPEED;
            PlayerQuickAccess.KINEMATIC_BODY.Velocity = Variables.Instance.WALKING_MOVEMENT + Variables.Instance.GRAVITY_MOVEMENT;
            PlayerQuickAccess.KINEMATIC_BODY.MoveAndSlide();
            float gravitySqr = Variables.Instance.GRAVITY_MOVEMENT.LengthSquared();
            if (gravitySqr > 1f)
            {
                Variables.Instance.GRAVITY_MOVEMENT -= Variables.Instance.GRAVITY_MOVEMENT * ((float)delta) * Mathf.Clamp(30 - gravitySqr, 1, 30);

                if (Variables.Instance.GRAVITY_MOVEMENT.LengthSquared() < 1f)
                {
                    Variables.Instance.GRAVITY_MOVEMENT = Vector3.Zero;
                }
            }
        }

        private Vector3 DirectionMovement()
        {
            Vector3 dir = (InputToInt("ui_right") - InputToInt("ui_left")) * PlayerQuickAccess.CAMERA.GlobalTransform.Basis.X;
            dir += (InputToInt("ui_down") - InputToInt("ui_up")) * PlayerQuickAccess.CAMERA.GlobalTransform.Basis.Z;
            return dir.Normalized();
        }

        public override void FloorDetection()
        {

        }

        public override void Movement(double delta)
        {

        }

        public override void Starting()
        {
            Variables.Instance.ON_FLOOR = false;
            PlayerQuickAccess.KINEMATIC_BODY.MotionMode = CharacterBody3D.MotionModeEnum.Floating;
        }
    }

}
