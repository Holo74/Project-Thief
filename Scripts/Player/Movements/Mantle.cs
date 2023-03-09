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
            // PlayerQuickAccess.CHARACTER_BODY.UpDirection = Vector3.Zero;
            PlayerQuickAccess.CHARACTER_BODY.MotionMode = CharacterBody3D.MotionModeEnum.Floating;
            Variables.Instance.ON_FLOOR = false;
            Variables.Instance.WALKING_MOVEMENT = -PlayerQuickAccess.BODY_DIRECTION.Z * ((float)Variables.Instance.MANTLE_FORWARD_SPEED);
            Variables.Instance.GRAVITY_MOVEMENT = Vector3.Up * ((float)Variables.Instance.MANTLE_UPWARD_SPEED);
            WasCrouched = Helper.CommonComparisions.IS_CROUCHED;
            Timer = Variables.Instance.MANTLE_UPWARD_TIME;
            CurrentMoving = UpwardMoving;
            Variables.Instance.ROTATION = null;
            LedgeAboveWaist = PlayerQuickAccess.MANTLE.UpperLedge.IsColliding();
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
            KinematicCollision3D k = PlayerQuickAccess.CHARACTER_BODY.MoveAndCollide(Variables.Instance.GRAVITY_MOVEMENT * ((float)delta));
            if ((PlayerQuickAccess.MANTLE.CanMoveForwardUpper() && LedgeAboveWaist) || PlayerQuickAccess.MANTLE.CanMoveForwardLower())
            {
                //GD.Print(String.Format("Above waist Ledge: {0}\nUpper forward:{1}\nLower forward:{2}\n", LedgeAboveWaist, PlayerQuickAccess.MANTLE.CanMoveForwardUpper(), PlayerQuickAccess.MANTLE.CanMoveForwardLower()));
                TransitionToForward();
            }
            if (k != null)
            {
                if (k.GetPosition().Y > PlayerQuickAccess.CHARACTER_BODY.GlobalPosition.Y + 1f)
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
            if (Timer < 0)
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
            KinematicCollision3D k = PlayerQuickAccess.CHARACTER_BODY.MoveAndCollide(Variables.Instance.WALKING_MOVEMENT * ((float)delta));
            if (k != null)
            {
                Vector3 holder = k.GetPosition();
                holder.Y = PlayerQuickAccess.CHARACTER_BODY.GlobalPosition.Y;
                if (holder.DistanceTo(PlayerQuickAccess.CHARACTER_BODY.GlobalPosition) > .5f)
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
            KinematicCollision3D k = PlayerQuickAccess.CHARACTER_BODY.MoveAndCollide(Vector3.Down * 10 * ((float)delta));
            if (k != null)
            {
                FinishMantle();
            }
        }

        private void FinishMantle()
        {
            Variables.Instance.WALKING_MOVEMENT = Vector3.Zero;
            Variables.Instance.GRAVITY_MOVEMENT = Vector3.Zero;
            if (!WasCrouched && WasCrouched != Helper.CommonComparisions.IS_CROUCHED)
            {
                CrouchWithoutInput();
            }
            PlayerQuickAccess.CHARACTER_BODY.MotionMode = CharacterBody3D.MotionModeEnum.Grounded;

            Variables.Instance.RESET_MOVEMENT();
        }
    }

}
