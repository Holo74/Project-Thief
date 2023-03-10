using Godot;
using System;
using System.Threading.Tasks;

namespace Player.Movement
{
    public abstract class AbstractMovement
    {
        #region Basic Movement
        public abstract void Movement(float delta);

        public virtual void FallingMovement(float delta)
        {
            if (Mantled(delta))
            {
                return;
            }
            if (Input.IsActionJustPressed("ui_select"))
            {
                if (PlayerQuickAccess.KINEMATIC_BODY.IsOnWall() && Variables.Instance.WALKING_MOVEMENT.LengthSquared() > .1f)
                {
                    Variables.Instance.GRAVITY_MOVEMENT += Vector3.Up * Variables.Instance.BOOST_WALL_JUMP;
                    Variables.Instance.WALKING_MOVEMENT = Vector3.Zero;
                }
            }
            Vector3 move = DirectionalInput() * MovementSpeed();
            Variables.Instance.MOVEMENT.Crouch();
            PlayerQuickAccess.KINEMATIC_BODY.MoveAndSlide(move + Variables.Instance.GRAVITY_MOVEMENT, Vector3.Up);
            Variables.Instance.GRAVITY_MOVEMENT += delta * Vector3.Down * Variables.Instance.GRAVITY_STRENGTH * Variables.Instance.GRAVITY_MOD;
        }
        #endregion

        private float MantleTimer { get; set; }
        protected bool Mantled(float delta)
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
                    if (MantleTimer > Variables.Instance.MANTLE_BUFFER_TIMER)
                    {
                        Variables.Instance.MOVEMENT = new Movement.Mantle();
                        return true;
                    }
                }
            }
            return false;
        }

        public virtual void FloorDetection()
        {
            if (PlayerQuickAccess.KINEMATIC_BODY.IsOnFloor() != Variables.Instance.ON_FLOOR)
            {
                // GD.Print("Changing floor too: " + PlayerQuickAccess.KINEMATIC_BODY.IsOnFloor());
                Variables.Instance.ON_FLOOR = PlayerQuickAccess.KINEMATIC_BODY.IsOnFloor();
                GD.Print("Gravity: " + Variables.Instance.GRAVITY_MOVEMENT);
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
            if (Variables.Instance.CURRENT_STANDING_STATE == Variables.PlayerStandingState.Crawling)
            {
                return;
            }
            Vector3 holder = Variables.Instance.GRAVITY_MOVEMENT;
            holder.y = Variables.Instance.JUMP_STRENGTH;
            holder += modifier;
            Variables.Instance.GRAVITY_MOVEMENT = holder * Variables.Instance.JUMP_MOD;
            Variables.Instance.Jump?.Invoke();
        }

        protected int InputToInt(string input)
        {
            return Input.IsActionPressed(input) ? 1 : 0;
        }

        protected float MovementSpeed()
        {
            Variables.Instance.IS_SPRINTING = Input.IsActionPressed("Run");
            return Helper.MathEquations.GET_SPEED();
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
            if (Variables.Instance.ON_FLOOR)
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
            if (Helper.CommonComparisions.IS_CROUCHED)
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
                StateChangeTo(Variables.PlayerStandingState.Crouching);
                // PlayerQuickAccess.DisableLowerBody(true);
                // Variables.Instance.CURRENT_STANDING_STATE = Variables.PlayerStandingState.Crouching;
                Variables.Instance.OnFloorChange += CrouchGroundCorrection;

            }
        }

        private void CrouchGroundCorrection(bool onFloor)
        {
            if (onFloor)
            {
                PlayerQuickAccess.CAMERA.Translation = Vector3.Down * .1f;
                PlayerQuickAccess.KINEMATIC_BODY.Translate(Vector3.Up);
                UpdateCollision();
                Variables.Instance.OnFloorChange -= CrouchGroundCorrection;

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
            Variables.Instance.CURRENT_STANDING_STATE = state;
            UpdateCollision();
            CrouchCamera();
        }

        private Vector3 FloorCorrection()
        {
            Vector3 totalMove = Variables.Instance.WALKING_MOVEMENT;
            Vector3 normal = PlayerQuickAccess.FLOOR_CAST.GetCollisionNormal();
            if (!PlayerQuickAccess.FLOOR_CAST.IsColliding() || normal.Dot(Vector3.Down) < 0.2f)
            {
                return totalMove;
            }
            totalMove = totalMove.Cross(Vector3.Down);
            totalMove = totalMove.Cross(normal);
            return totalMove;
        }

        private void UpdateCollision()
        {
            switch (Variables.Instance.CURRENT_STANDING_STATE)
            {
                case Variables.PlayerStandingState.Standing:
                    PlayerQuickAccess.UPPER_BODY.Disabled = false;
                    PlayerQuickAccess.LOWER_BODY.Disabled = false;
                    PlayerQuickAccess.FEET.Disabled = false;
                    break;
                case Variables.PlayerStandingState.Crouching:
                    if (Variables.Instance.ON_FLOOR)
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
            state = (Variables.Instance.CURRENT_STANDING_STATE == state) ? Variables.PlayerStandingState.Standing : state;
            if (Helper.StateChangeChecker.TransferToState(state))
            {
                SetStandingState(state);
            }
        }

        private void CrouchCamera()
        {
            PlayerQuickAccess.TWEEN.Kill();
            switch (Variables.Instance.CURRENT_STANDING_STATE)
            {
                case Variables.PlayerStandingState.Standing:
                    PlayerQuickAccess.CreateCameraTween().TweenProperty(PlayerQuickAccess.CAMERA, "translation:y", .9, Mathf.Abs(.9f - PlayerQuickAccess.CAMERA.Translation.y) / Variables.Instance.MOVE_TO_CROUCH);
                    break;
                case Variables.PlayerStandingState.Crouching:
                    if (Variables.Instance.ON_FLOOR)
                    {
                        PlayerQuickAccess.CreateCameraTween().TweenProperty(PlayerQuickAccess.CAMERA, "translation:y", -.1f, Mathf.Abs(-.1f - PlayerQuickAccess.CAMERA.Translation.y) / Variables.Instance.MOVE_TO_CROUCH);
                    }
                    break;
                case Variables.PlayerStandingState.Crawling:
                    PlayerQuickAccess.CreateCameraTween().TweenProperty(PlayerQuickAccess.CAMERA, "translation:y", -.6f, Mathf.Abs(-.6f - PlayerQuickAccess.CAMERA.Translation.y) / Variables.Instance.MOVE_TO_CROUCH);
                    break;
            }
        }

        protected Vector3 TotalMovement()
        {
            if (Variables.Instance.ON_FLOOR)
            {
                return FloorCorrection() + Variables.Instance.GRAVITY_MOVEMENT;
            }
            return Variables.Instance.WALKING_MOVEMENT + Variables.Instance.GRAVITY_MOVEMENT;
        }

        protected virtual void FloorStateChange(bool state)
        {
            if (state)
            {
                // Variables.Instance.GRAVITY_MOVEMENT = Vector3.Zero;
            }
            else
            {
                if (Variables.Instance.CURRENT_STANDING_STATE == Variables.PlayerStandingState.Crawling)
                {
                    // This doesn't work for some reason.  I clip through the ground
                    //CrouchWithoutInput();
                }
            }
        }

        public virtual void Starting()
        {
            Variables.Instance.RESET_ROTATION();
            Variables.Instance.OnFloorChange += FloorStateChange;
        }
    }

}
