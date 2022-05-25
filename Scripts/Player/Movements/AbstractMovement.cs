using Godot;
using System;
using System.Threading.Tasks;

namespace Player.Movement
{
    public abstract class AbstractMovement
    {
        private Vector3 DebugStartingPos { get; set; }
        private bool FallingLock { get; set; } = false;
        protected bool currentFloorState = false;
        public abstract void Movement(float delta);
        public Rotation.BasicRotation Rotation { get; set; }

        public virtual void FallingMovement(float delta)
        {
            if (PlayerQuickAccess.KINEMATIC_BODY.IsOnWall() && Input.IsActionPressed("ui_select") && Variables.WALKING_MOVEMENT.LengthSquared() > .1f)
            {
                Variables.GRAVITY_MOVEMENT += Vector3.Up * Variables.BOOST_WALL_JUMP;
                Variables.WALKING_MOVEMENT = Vector3.Zero;
            }
            Vector3 move = DirectionalInput() * MovementSpeed();
            Variables.MOVEMENT.Crouch();
            PlayerQuickAccess.KINEMATIC_BODY.MoveAndSlide(move + Variables.GRAVITY_MOVEMENT, Vector3.Up);
            Variables.GRAVITY_MOVEMENT += delta * Vector3.Down * Variables.GRAVITY_STRENGTH * Variables.GRAVITY_MOD;
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
            if (Variables.IS_CROUCHED)
            {
                current = Variables.CROUCH_SPEED;
            }
            return current * Variables.SPEED_MOD;
        }

        public void Crouch()
        {
            if (Input.IsActionJustPressed("crouch"))
            {
                if (Variables.ON_FLOOR)
                {
                    GroundCrouch();
                }
                else
                {
                    AirCrouch();
                }
            }
        }

        private bool standOnLand = false;
        private void AirCrouch()
        {
            if (Variables.IS_CROUCHED)
            {
                if (PlayerQuickAccess.LOWER_BODY.Disabled)
                {
                    standOnLand = !standOnLand;
                }
                else
                {
                    GroundCrouch();
                }
            }
            else
            {
                PlayerQuickAccess.LOWER_BODY.Disabled = true;
                Variables.IS_CROUCHED = true;
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
                PlayerQuickAccess.LOWER_BODY.Disabled = false;
                Variables.OnFloorChange -= CrouchGroundCorrection;

                if (standOnLand)
                {
                    DelayedCrouching();
                }
            }
        }

        private async void DelayedCrouching()
        {
            await Task.Delay(100);
            standOnLand = false;
            GroundCrouch();
        }

        private void GroundCrouch()
        {
            if (Variables.IS_CROUCHED)
            {
                if (PlayerQuickAccess.UPPER_BODY_AREA.GetOverlappingBodies().Count == 0)
                {
                    Variables.IS_CROUCHED = false;
                    PlayerQuickAccess.UPPER_BODY.Disabled = false;
                    CrouchCamera();
                }
            }
            else
            {
                Variables.IS_CROUCHED = true;
                PlayerQuickAccess.UPPER_BODY.Disabled = true;
                CrouchCamera();
            }
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

        private void CrouchCamera()
        {
            PlayerQuickAccess.TWEEN.Stop(PlayerQuickAccess.CAMERA, "translation.y");
            if (Variables.IS_CROUCHED)
            {
                PlayerQuickAccess.TWEEN.InterpolateProperty(PlayerQuickAccess.CAMERA, "translation:y", PlayerQuickAccess.CAMERA.Translation.y, -.1f, (PlayerQuickAccess.CAMERA.Translation.y + .1f) / Variables.CROUCHING_SPEED);
            }
            else
            {
                PlayerQuickAccess.TWEEN.InterpolateProperty(PlayerQuickAccess.CAMERA, "translation:y", PlayerQuickAccess.CAMERA.Translation.y, .9, (.9f - PlayerQuickAccess.CAMERA.Translation.y) / Variables.CROUCHING_SPEED);
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

        public virtual void Starting()
        {
            //Variables.OnFloorChange += DebugLanding;
        }
    }

}
