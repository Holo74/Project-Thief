using Godot;
using System;

namespace Player.Movement
{
    public class Swimming : AbstractMovement
    {
        public override void FallingMovement(float delta)
        {
            Variables.WALKING_MOVEMENT = DirectionMovement() * Variables.SPEED;
            PlayerQuickAccess.KINEMATIC_BODY.MoveAndSlide(Variables.WALKING_MOVEMENT + Variables.GRAVITY_MOVEMENT);
            float gravitySqr = Variables.GRAVITY_MOVEMENT.LengthSquared();
            if (gravitySqr > 1f)
            {
                Variables.GRAVITY_MOVEMENT -= Variables.GRAVITY_MOVEMENT * delta * Mathf.Clamp(30 - gravitySqr, 1, 30);

                if (Variables.GRAVITY_MOVEMENT.LengthSquared() < 1f)
                {
                    Variables.GRAVITY_MOVEMENT = Vector3.Zero;
                }
            }
        }

        private Vector3 DirectionMovement()
        {
            Vector3 dir = (InputToInt("ui_right") - InputToInt("ui_left")) * PlayerQuickAccess.CAMERA.GlobalTransform.basis.x;
            dir += (InputToInt("ui_down") - InputToInt("ui_up")) * PlayerQuickAccess.CAMERA.GlobalTransform.basis.z;
            return dir.Normalized();
        }

        public override void FloorDetection()
        {

        }

        public override void Movement(float delta)
        {

        }

        public override void Starting()
        {
            Variables.ON_FLOOR = false;
        }
    }

}
