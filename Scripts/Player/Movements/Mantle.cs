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
        private Transform3D StartingTransform { get; set; }
        private float Interpolating { get; set; }

        private Vector3 RightAngle { get; set; }
        public override void Starting()
        {
            // PlayerQuickAccess.CAMERA_SHAKE.Shake(0, 0, Mathf.Clamp(Variables.Instance.WALKING_MOVEMENT.Length(), 0f, 0.5f), .1f, 1, 1, -1);

            Variables.Instance.ON_FLOOR = false;
            Variables.Instance.WALKING_MOVEMENT = -PlayerQuickAccess.BODY_DIRECTION.Z * Variables.Instance.MANTLE_FORWARD_SPEED;
            Variables.Instance.GRAVITY_MOVEMENT = Vector3.Up * Variables.Instance.MANTLE_UPWARD_SPEED;
            WasCrouched = Helper.CommonComparisions.IS_CROUCHED;
            Timer = Variables.Instance.MANTLE_UPWARD_TIME;
            if (PlayerQuickAccess.MANTLE.HandsAboveWaist())
            {
                Interpolating = 0f;
                StartingTransform = PlayerQuickAccess.KINEMATIC_BODY.GlobalTransform;
                CurrentMoving = RotateToFaceLedge;

            }
            else
            {
                CurrentMoving = UpwardMoving;
            }

            Variables.Instance.ROTATION = null;
            // LedgeAboveWaist = PlayerQuickAccess.MANTLE.UpperLedge.IsColliding();
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

        private void RotateToFaceLedge(double delta)
        {
            Interpolating += ((float)delta) * 2;
            Transform3D holder = PlayerQuickAccess.KINEMATIC_BODY.GlobalTransform.LookingAt(PlayerQuickAccess.KINEMATIC_BODY.GlobalPosition - PlayerQuickAccess.MANTLE.RightHand.GetCollisionNormal(), Vector3.Up);
            PlayerQuickAccess.KINEMATIC_BODY.GlobalTransform = StartingTransform.InterpolateWith(holder, Interpolating);
            if (Interpolating > 1.0f)
            {
                CurrentMoving = MoveToWall;
                PlayerQuickAccess.KINEMATIC_BODY.UpDirection = PlayerQuickAccess.MANTLE.RightHand.GetCollisionNormal();
                RightAngle = Vector3.Up.Cross(-PlayerQuickAccess.BODY_DIRECTION.Z);
            }
        }

        private void MoveToWall(double delta)
        {
            float dis = PlayerQuickAccess.MANTLE.Upper.GetSmallestDistance();
            Vector3 moveToWall = -PlayerQuickAccess.MANTLE.RightHand.GetCollisionNormal() * 10;
            moveToWall += Vector3.Down * ((dis < .1f) ? 0 : dis) * 2;
            PlayerQuickAccess.KINEMATIC_BODY.Velocity = moveToWall;
            PlayerQuickAccess.KINEMATIC_BODY.MoveAndSlide();
            if (dis < .1f && PlayerQuickAccess.KINEMATIC_BODY.IsOnFloor())
            {
                CurrentMoving = MoveAlongLedge;
                Variables.Instance.ROTATION = new Rotation.HeadOnly();
            }
        }

        private Vector3 PreviousMove { get; set; }
        private void MoveAlongLedge(double delta)
        {
            PlayerQuickAccess.KINEMATIC_BODY.Velocity = Vector3.Zero;
            if (PlayerQuickAccess.MANTLE.Upper.RightMost.IsColliding() && Input.IsActionPressed("ui_right"))
            {
                PlayerQuickAccess.KINEMATIC_BODY.Velocity = PlayerQuickAccess.MANTLE.Upper.GetCasterAngle(true) * Vector3.Up;
                PlayerQuickAccess.KINEMATIC_BODY.Velocity -= RightAngle;
            }
            if (PlayerQuickAccess.MANTLE.Upper.LeftMost.IsColliding() && Input.IsActionPressed("ui_left"))
            {
                PlayerQuickAccess.KINEMATIC_BODY.Velocity = PlayerQuickAccess.MANTLE.Upper.GetCasterAngle(false) * Vector3.Up;
                PlayerQuickAccess.KINEMATIC_BODY.Velocity += RightAngle;
            }

            if (Input.IsActionJustPressed("ui_select"))
            {
                Jump(-PlayerQuickAccess.CAMERA.GlobalTransform.Basis.Z);
                ResetHeadAndBodyRotation();
                Variables.Instance.MOVEMENT = new BasicMovement();
            }

            PlayerQuickAccess.KINEMATIC_BODY.MoveAndSlide();
        }

        private void ResetHeadAndBodyRotation()
        {
            Vector3 rotation = PlayerQuickAccess.CAMERA.Rotation;
            PlayerQuickAccess.CAMERA.RotateY(-rotation.Y);
            PlayerQuickAccess.KINEMATIC_BODY.RotateY(rotation.Y);
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
