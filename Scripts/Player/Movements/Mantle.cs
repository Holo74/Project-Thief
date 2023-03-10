using Godot;
using System;

namespace Player.Movement
{
    public class Mantle : AbstractMovement
    {
        public delegate void Moving(float delta);
        private Moving CurrentMoving;
        private bool WasCrouched { get; set; }
        private float Timer { get; set; }
        private bool LedgeAboveWaist { get; set; }

        public override void Starting()
        {
            // PlayerQuickAccess.CAMERA_SHAKE.Shake(0, 0, Mathf.Clamp(Variables.Instance.WALKING_MOVEMENT.Length(), 0f, 0.5f), .1f, 1, 1, -1);

            Variables.Instance.ON_FLOOR = false;
            Variables.Instance.WALKING_MOVEMENT = -PlayerQuickAccess.BODY_DIRECTION.z * Variables.Instance.MANTLE_FORWARD_SPEED;
            Variables.Instance.GRAVITY_MOVEMENT = Vector3.Up * Variables.Instance.MANTLE_UPWARD_SPEED;
            WasCrouched = Helper.CommonComparisions.IS_CROUCHED;
            Timer = Variables.Instance.MANTLE_UPWARD_TIME;
            CurrentMoving = UpwardMoving;
            Variables.Instance.ROTATION = null;
            LedgeAboveWaist = PlayerQuickAccess.MANTLE.UpperLedge.IsColliding();
        }

        public override void Movement(float delta)
        {
            MantleUp(delta);
        }

        public override void FallingMovement(float delta)
        {
            MantleUp(delta);
        }

        private void MantleUp(float delta)
        {
            CurrentMoving(delta);
        }

        private void UpwardMoving(float delta)
        {
            KinematicCollision k = PlayerQuickAccess.KINEMATIC_BODY.MoveAndCollide(Variables.Instance.GRAVITY_MOVEMENT * delta);
            if ((PlayerQuickAccess.MANTLE.CanMoveForwardUpper() && LedgeAboveWaist) || PlayerQuickAccess.MANTLE.CanMoveForwardLower())
            {
                //GD.Print(String.Format("Above waist Ledge: {0}\nUpper forward:{1}\nLower forward:{2}\n", LedgeAboveWaist, PlayerQuickAccess.MANTLE.CanMoveForwardUpper(), PlayerQuickAccess.MANTLE.CanMoveForwardLower()));
                TransitionToForward();
            }
            if (k != null)
            {
                if (k.Position.y > PlayerQuickAccess.KINEMATIC_BODY.GlobalTransform.origin.y + 1f)
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

        private void ForwardMove(float delta)
        {
            KinematicCollision k = PlayerQuickAccess.KINEMATIC_BODY.MoveAndCollide(Variables.Instance.WALKING_MOVEMENT * delta);
            if (k != null)
            {
                Vector3 holder = k.Position;
                holder.y = PlayerQuickAccess.KINEMATIC_BODY.GlobalTransform.origin.y;
                if (holder.DistanceTo(PlayerQuickAccess.KINEMATIC_BODY.GlobalTransform.origin) > .5f)
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

        private void MoveToGround(float delta)
        {
            KinematicCollision k = PlayerQuickAccess.KINEMATIC_BODY.MoveAndCollide(Vector3.Down * 10 * delta);
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
            Variables.Instance.RESET_MOVEMENT();
        }
    }

}
