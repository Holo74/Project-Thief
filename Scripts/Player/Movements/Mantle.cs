using Godot;
using System;

namespace Player.Movement
{
    public partial class Mantle : AbstractMovement
    {
        public delegate void Moving(double delta);
        private Moving CurrentMoving;
        private bool WasCrouched { get; set; }
        private double Timer { get; set; }
        private bool LedgeAboveWaist { get; set; }

        public override void Starting()
        {
            // PlayerQuickAccess.CAMERA_SHAKE.Shake(0, 0, Mathf.Clamp(Variables.Instance.WALKING_MOVEMENT.Length(), 0f, 0.5f), .1f, 1, 1, -1);

            Variables.Instance.ON_FLOOR = false;
            Variables.Instance.WALKING_MOVEMENT = -PlayerQuickAccess.BODY_DIRECTION.Z * Variables.Instance.MANTLE_FORWARD_SPEED;
            Variables.Instance.GRAVITY_MOVEMENT = Vector3.Up * Variables.Instance.MANTLE_UPWARD_SPEED;
            WasCrouched = Helper.CommonComparisions.IS_CROUCHED;
            Timer = Variables.Instance.MANTLE_UPWARD_TIME;
            CurrentMoving = UpwardMoving;
            Variables.Instance.ROTATION = null;
            // LedgeAboveWaist = PlayerQuickAccess.MANTLE.UpperLedge.IsColliding();
            LedgeAboveWaist = false;
        }

        public override void Movement(double delta)
        {
            MantleUp(delta);
        }

        public override void FallingMovement(double delta)
        {
            MantleUp(delta);
        }

        private void MantleUp(double delta)
        {
            CurrentMoving(delta);
        }

        private void UpwardMoving(double delta)
        {
            KinematicCollision3D k = PlayerQuickAccess.KINEMATIC_BODY.MoveAndCollide(Variables.Instance.GRAVITY_MOVEMENT * ((float)delta));
            if (k != null)
            {
                if (k.GetPosition().Y > PlayerQuickAccess.KINEMATIC_BODY.GlobalPosition.Y + 1f)
                {
                    if (!Helper.CommonComparisions.IS_CROUCHED)
                    {
                        CrouchWithoutInput();
                        //GD.Print("Crouched when going up");
                    }
                    else
                    {
                        TransitionToForward();
                    }
                }
            }
            Timer -= delta;
            if (Timer < 0 || PlayerQuickAccess.KINEMATIC_BODY.GlobalPosition.Y > PlayerQuickAccess.MANTLE.MantleHeight.Y)
            {
                //GD.Print("Upper timer ran out");
                TransitionToForward();
            }
        }

        private void TransitionToForward()
        {
            CurrentMoving = ForwardMove;
            Variables.Instance.GRAVITY_MOVEMENT = Vector3.Zero;
            Timer = Variables.Instance.MANTLE_FORWARD_TIME;
        }

        private void ForwardMove(double delta)
        {
            KinematicCollision3D k = PlayerQuickAccess.KINEMATIC_BODY.MoveAndCollide(Variables.Instance.WALKING_MOVEMENT * ((float)delta));
            if (k != null)
            {
                Vector3 holder = k.GetPosition();
                holder.Y = PlayerQuickAccess.KINEMATIC_BODY.GlobalPosition.Y;
                if (holder.DistanceTo(PlayerQuickAccess.KINEMATIC_BODY.GlobalPosition) > .5f)
                {
                    if (!Helper.CommonComparisions.IS_CROUCHED)
                    {
                        //GD.Print("Crouched when moving forward");
                        CrouchWithoutInput();
                    }
                    else
                    {
                        FinishMantle();
                    }
                }
            }
            Timer -= delta;
            if (Timer < 0)
            {
                CurrentMoving = MoveToGround;
            }
        }

        private void MoveToGround(double delta)
        {
            KinematicCollision3D k = PlayerQuickAccess.KINEMATIC_BODY.MoveAndCollide(Vector3.Down * 10 * ((float)delta));
            if (k != null)
            {
                FinishMantle();
            }
        }

        private void FinishMantle()
        {
            Variables.Instance.WALKING_MOVEMENT = Vector3.Zero;
            Variables.Instance.GRAVITY_MOVEMENT = Vector3.Zero;
            PlayerQuickAccess.KINEMATIC_BODY.MotionMode = CharacterBody3D.MotionModeEnum.Grounded;
            if (!WasCrouched && WasCrouched != Helper.CommonComparisions.IS_CROUCHED)
            {
                CrouchWithoutInput();
            }
            Variables.Instance.RESET_MOVEMENT();
        }
    }

}
