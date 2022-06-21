using Godot;
using System;
using System.Threading.Tasks;

namespace Player.Movement
{
    public abstract class AbstractMovement
    {
        private Vector3 DebugStartingPos { get; set; }
        private bool FallingLock { get; set; } = false;

        public abstract void Movement(float delta);

        public virtual void FallingMovement(float delta)
        {
            if (Mantled(delta))
            {
                return;
            }
            if (Input.IsActionJustPressed("ui_select"))
            {
                if (PlayerQuickAccess.KINEMATIC_BODY.IsOnWall() && Variables.WALKING_MOVEMENT.LengthSquared() > .1f)
                {
                    Variables.GRAVITY_MOVEMENT += Vector3.Up * Variables.BOOST_WALL_JUMP;
                    Variables.WALKING_MOVEMENT = Vector3.Zero;
                }
            }
            Vector3 move = DirectionalInput() * MovementSpeed();
            Variables.MOVEMENT.Crouch();
            PlayerQuickAccess.KINEMATIC_BODY.MoveAndSlide(move + Variables.GRAVITY_MOVEMENT, Vector3.Up);
            Variables.GRAVITY_MOVEMENT += delta * Vector3.Down * Variables.GRAVITY_STRENGTH * Variables.GRAVITY_MOD;
        }

        private float MantleTimer { get; set; }
        private bool Mantled(float delta)
        {
            if (Input.IsActionJustPressed("ui_select"))
            {
                MantleTimer = 0f;
            }
            if (Input.IsActionPressed("ui_select"))
            {
                MantleTimer += delta;
                if (PlayerQuickAccess.MANTLE.CanMantle())
                {
                    if (MantleTimer > Variables.MANTLE_BUFFER_TIMER)
                    {
                        Variables.MOVEMENT = new Movement.Mantle();
                        return true;
                    }
                }
            }
            return false;
        }

        public virtual void FloorDetection()
        {
            if (PlayerQuickAccess.KINEMATIC_BODY.IsOnFloor() != Variables.ON_FLOOR)
            {
                Variables.ON_FLOOR = PlayerQuickAccess.KINEMATIC_BODY.IsOnFloor();
            }
        }

        protected Vector3 DirectionalInput()
        {
            Vector3 output = new Vector3();
            output = (InputToInt("ui_down") - InputToInt("ui_up")) * PlayerQuickAccess.BODY_DIRECTION.z;
            output += (InputToInt("ui_right") - InputToInt("ui_left")) * PlayerQuickAccess.BODY_DIRECTION.x;
            return output.Normalized();
        }

        public void Jump(Vector3 modifier)
        {
            DebugStartingPos = PlayerQuickAccess.KINEMATIC_BODY.GlobalTransform.origin;
            Vector3 holder = Variables.GRAVITY_MOVEMENT;
            holder.y = Variables.JUMP_STRENGTH;
            holder += modifier;
            Variables.GRAVITY_MOVEMENT = holder * Variables.JUMP_MOD;
        }

        protected int InputToInt(string input)
        {
            return Input.IsActionPressed(input) ? 1 : 0;
        }

        protected float MovementSpeed()
        {
            Variables.IS_SPRINTING = Input.IsActionPressed("Run");
            float current = Variables.SPEED;
            if (Variables.IS_SPRINTING)
            {
                current = Variables.SPRINT_SPEED;
            }
            if (Variables.IS_CROUCHED && Variables.ON_FLOOR)
            {
                current = Variables.CROUCH_SPEED;
            }
            return current * Variables.SPEED_MOD;
        }

        public void Crouch()
        {
            if (Input.IsActionJustPressed("crouch"))
            {
                CrouchWithoutInput();
            }
        }

        public void CrouchWithoutInput()
        {
            if (Variables.ON_FLOOR)
            {
                StateChangeTo(Variables.PlayerStandingState.Crouching);
            }
            else
            {
                AirCrouch();
            }
        }

        private bool standOnLand = false;
        private void AirCrouch()
        {
            if (Variables.IS_CROUCHED)
            {
                if (PlayerQuickAccess.FEET.Disabled)
                {
                    standOnLand = !standOnLand;
                }
                else
                {
                    StateChangeTo(Variables.PlayerStandingState.Crouching);
                }
            }
            else
            {
                PlayerQuickAccess.DisableLowerBody(true);
                Variables.CURRENT_STANDING_STATE = Variables.PlayerStandingState.Crouching;
                Variables.OnFloorChange += CrouchGroundCorrection;
            }
        }

        private void CrouchGroundCorrection(bool onFloor)
        {
            if (onFloor)
            {
                PlayerQuickAccess.UPPER_BODY.Disabled = true;
                PlayerQuickAccess.CAMERA.Translation = Vector3.Down * .1f;
                PlayerQuickAccess.KINEMATIC_BODY.Translate(Vector3.Up);
                PlayerQuickAccess.DisableLowerBody(false);
                Variables.OnFloorChange -= CrouchGroundCorrection;

                if (standOnLand)
                {
                    DelayedCrouching();
                }
            }
        }

        private async void DelayedCrouching()
        {
            await Task.Delay(50);
            standOnLand = false;
            StateChangeTo(Variables.PlayerStandingState.Crouching);
        }

        private void SetStandingState(Variables.PlayerStandingState state)
        {
            Variables.CURRENT_STANDING_STATE = state;
            UpdateCollision();
            CrouchCamera();
        }

        private Vector3 FloorCorrection()
        {
            Vector3 totalMove = Variables.WALKING_MOVEMENT;
            if (!PlayerQuickAccess.FLOOR_CAST.IsColliding())
            {
                return totalMove;
            }
            totalMove = totalMove.Cross(Vector3.Down);
            totalMove = totalMove.Cross(PlayerQuickAccess.FLOOR_CAST.GetCollisionNormal());
            return totalMove;
        }

        private void UpdateCollision()
        {
            switch (Variables.CURRENT_STANDING_STATE)
            {
                case Variables.PlayerStandingState.Standing:
                    PlayerQuickAccess.UPPER_BODY.Disabled = false;
                    PlayerQuickAccess.LOWER_BODY.Disabled = false;
                    PlayerQuickAccess.FEET.Disabled = false;
                    break;
                case Variables.PlayerStandingState.Crouching:
                    if (Variables.ON_FLOOR)
                    {
                        PlayerQuickAccess.UPPER_BODY.Disabled = true;
                        PlayerQuickAccess.LOWER_BODY.Disabled = false;
                        PlayerQuickAccess.FEET.Disabled = false;
                    }
                    else
                    {
                        PlayerQuickAccess.UPPER_BODY.Disabled = false;
                        PlayerQuickAccess.LOWER_BODY.Disabled = true;
                        PlayerQuickAccess.FEET.Disabled = true;
                    }
                    break;
                case Variables.PlayerStandingState.Crawling:
                    PlayerQuickAccess.UPPER_BODY.Disabled = true;
                    PlayerQuickAccess.LOWER_BODY.Disabled = true;
                    PlayerQuickAccess.FEET.Disabled = false;
                    break;
            }
        }

        public void Crawl()
        {
            if (Input.IsActionJustPressed("crawl"))
            {
                StateChangeTo(Variables.PlayerStandingState.Crawling);
            }
        }

        public void StateChangeTo(Variables.PlayerStandingState state)
        {
            state = (Variables.CURRENT_STANDING_STATE == state) ? Variables.PlayerStandingState.Standing : state;
            if (Helper.StateChangeChecker.TransferToState(state))
            {
                SetStandingState(state);
            }
        }

        private void CrouchCamera()
        {
            PlayerQuickAccess.TWEEN.Stop(PlayerQuickAccess.CAMERA, "translation.y");
            switch (Variables.CURRENT_STANDING_STATE)
            {
                case Variables.PlayerStandingState.Standing:
                    PlayerQuickAccess.TWEEN.InterpolateProperty(PlayerQuickAccess.CAMERA, "translation:y", PlayerQuickAccess.CAMERA.Translation.y, .9, Mathf.Abs(.9f - PlayerQuickAccess.CAMERA.Translation.y) / Variables.CROUCHING_SPEED);
                    break;
                case Variables.PlayerStandingState.Crouching:
                    if (Variables.ON_FLOOR)
                    {
                        PlayerQuickAccess.TWEEN.InterpolateProperty(PlayerQuickAccess.CAMERA, "translation:y", PlayerQuickAccess.CAMERA.Translation.y, -.1f, Mathf.Abs(-.1f - PlayerQuickAccess.CAMERA.Translation.y) / Variables.CROUCHING_SPEED);
                    }
                    break;
                case Variables.PlayerStandingState.Crawling:
                    PlayerQuickAccess.TWEEN.InterpolateProperty(PlayerQuickAccess.CAMERA, "translation:y", PlayerQuickAccess.CAMERA.Translation.y, -.6f, Mathf.Abs(-.6f - PlayerQuickAccess.CAMERA.Translation.y) / Variables.CROUCHING_SPEED);
                    break;
            }
            PlayerQuickAccess.TWEEN.Start();
        }

        protected Vector3 TotalMovement()
        {
            if (Variables.ON_FLOOR)
            {
                return FloorCorrection() + Variables.GRAVITY_MOVEMENT;
            }
            return Variables.WALKING_MOVEMENT + Variables.GRAVITY_MOVEMENT;
        }

        private void DebugLanding(bool onFloor)
        {
            if (onFloor)
            {
                FallingLock = false;
                GD.Print(PlayerQuickAccess.KINEMATIC_BODY.GlobalTransform.origin.DistanceTo(DebugStartingPos) + " Length");
            }

        }

        protected virtual void FloorStateChange(bool state)
        {
            if (state)
            {

            }
            else
            {

            }
        }

        public virtual void Starting()
        {
            Variables.RESET_ROTATION();
            Variables.OnFloorChange += FloorStateChange;
        }
    }

}
