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

        public override void Starting()
        {
            Variables.ON_FLOOR = false;
            Variables.WALKING_MOVEMENT = -PlayerQuickAccess.BODY_DIRECTION.z * 3f;
            Variables.GRAVITY_MOVEMENT = Vector3.Up * 2.5f;
            WasCrouched = Variables.IS_CROUCHED;
            Timer = 0.6f;
            CurrentMoving = UpwardMoving;
            Variables.ROTATION = null;
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
            if (PlayerQuickAccess.KINEMATIC_BODY.MoveAndCollide(Variables.GRAVITY_MOVEMENT * delta) != null)
            {
                if (!Variables.IS_CROUCHED)
                {
                    CrouchWithoutInput();
                }
                else
                {
                    TransitionToForward();
                }
            }
            Timer -= delta;
            if (Timer < 0)
            {
                TransitionToForward();
            }
        }

        private void TransitionToForward()
        {
            CurrentMoving = ForwardMove;
            Variables.GRAVITY_MOVEMENT = Vector3.Zero;
            Timer = 0.2f;
        }

        private void ForwardMove(float delta)
        {
            if (PlayerQuickAccess.KINEMATIC_BODY.MoveAndCollide(Variables.WALKING_MOVEMENT * delta) != null)
            {
                if (!Variables.IS_CROUCHED)
                {
                    CrouchWithoutInput();
                }
                else
                {
                    FinishMantle();
                }
            }
            Timer -= delta;
            if (Timer < 0)
            {
                FinishMantle();
            }
        }

        private void FinishMantle()
        {
            Variables.WALKING_MOVEMENT = Vector3.Zero;
            if (!WasCrouched && WasCrouched != Variables.IS_CROUCHED)
            {
                CrouchWithoutInput();
            }
            Variables.RESET_MOVEMENT();
        }
    }

}
