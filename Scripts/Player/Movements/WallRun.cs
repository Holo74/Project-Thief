using Godot;
using System;

namespace Player.Movement
{
    public partial class WallRun : AbstractMovement
    {
        protected bool currentFloorState = false;
        public override void Movement(double delta)
        {
            // Pushes the player onto a floor
            Vector3 jumpFactor = Vector3.Zero;
            Variables.Instance.WALKING_MOVEMENT = DirectionalInput();
            // This function is only active when the player is "on floor" and with walls as floor now we can assume this case
            if (!PlayerQuickAccess.KINEMATIC_BODY.IsOnFloor())
            {
                if (PlayerQuickAccess.WALL_DETECTION.IsWallDetected(false))
                {
                    PlayerQuickAccess.KINEMATIC_BODY.UpDirection = PlayerQuickAccess.WALL_DETECTION.GetLeftNormal();
                    jumpFactor = PlayerQuickAccess.WALL_DETECTION.GetLeftNormal();
                    Variables.Instance.WALKING_MOVEMENT = PlayerQuickAccess.KINEMATIC_BODY.UpDirection.Cross(Vector3.Down);
                }
                if (PlayerQuickAccess.WALL_DETECTION.IsWallDetected(true))
                {
                    PlayerQuickAccess.KINEMATIC_BODY.UpDirection = PlayerQuickAccess.WALL_DETECTION.GetRightNormal();
                    jumpFactor = PlayerQuickAccess.WALL_DETECTION.GetRightNormal();
                    Variables.Instance.WALKING_MOVEMENT = PlayerQuickAccess.KINEMATIC_BODY.UpDirection.Cross(Vector3.Up);
                }
            }
            Variables.Instance.WALKING_MOVEMENT *= MovementSpeed();
            if (Input.IsActionJustPressed("ui_select"))
            {
                Jump(jumpFactor * Variables.Instance.JUMP_STRENGTH);
            }
            if (Variables.Instance.GRAVITY_MOVEMENT.Y > 0)
            {
                PlayerQuickAccess.KINEMATIC_BODY.UpDirection = Vector3.Up;
            }
            PlayerQuickAccess.KINEMATIC_BODY.Velocity = TotalMovement();
            PlayerQuickAccess.KINEMATIC_BODY.MoveAndSlide();
        }

        // Really the only time I have to update the floor detection.  Just adding that walls are now floors lol
        public override void FloorDetection()
        {
            bool grounded = PlayerQuickAccess.KINEMATIC_BODY.IsOnFloor();
            // Walls are only floors if you are moving fast
            bool canRun = PlayerQuickAccess.WALL_DETECTION.IsWallDetected(true) || PlayerQuickAccess.WALL_DETECTION.IsWallDetected(false);
            canRun = Variables.Instance.WALKING_MOVEMENT.LengthSquared() > 1f ? canRun : false;
            grounded = grounded || (canRun);
            if (grounded != currentFloorState)
            {
                Variables.Instance.ON_FLOOR = grounded;
                currentFloorState = Variables.Instance.ON_FLOOR;
            }
        }
    }
}
