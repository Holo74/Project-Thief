using Godot;
using System;

namespace Player.Movement
{
    public partial class WallRun : AbstractMovement
    {
        private float Buffer = 0;
        protected bool currentFloorState = false;
        public override void Movement(double delta)
        {
            // The snap direction is determined by whether you're running on the wall or the floor
            Vector3 up = Vector3.Down;
            // Pushes the player onto a floor
            Vector3 jumpFactor = Vector3.Zero;
            Variables.Instance.WALKING_MOVEMENT = DirectionalInput();
            // This function is only active when the player is "on floor" and with walls as floor now we can assume this case
            if (!PlayerQuickAccess.CHARACTER_BODY.IsOnFloor())
            {
                if (PlayerQuickAccess.WALL_DETECTION.IsWallDetected(false))
                {
                    PlayerQuickAccess.CHARACTER_BODY.UpDirection = PlayerQuickAccess.WALL_DETECTION.GetLeftNormal();
                    jumpFactor = PlayerQuickAccess.WALL_DETECTION.GetLeftNormal();
                    Variables.Instance.WALKING_MOVEMENT = PlayerQuickAccess.CHARACTER_BODY.UpDirection.Cross(Vector3.Down);
                }
                if (PlayerQuickAccess.WALL_DETECTION.IsWallDetected(true))
                {
                    PlayerQuickAccess.CHARACTER_BODY.UpDirection = PlayerQuickAccess.WALL_DETECTION.GetRightNormal();
                    jumpFactor = PlayerQuickAccess.WALL_DETECTION.GetRightNormal();
                    Variables.Instance.WALKING_MOVEMENT = PlayerQuickAccess.CHARACTER_BODY.UpDirection.Cross(Vector3.Up);
                }
            }
            else
            {
                PlayerQuickAccess.CHARACTER_BODY.UpDirection = Vector3.Up;
            }
            Variables.Instance.WALKING_MOVEMENT *= ((float)MovementSpeed());
            if (Input.IsActionJustPressed("Jump"))
            {
                Jump(jumpFactor * ((float)Variables.Instance.JUMP_STRENGTH));
            }
            PlayerQuickAccess.CHARACTER_BODY.Velocity = TotalMovement();
            PlayerQuickAccess.CHARACTER_BODY.MoveAndSlide();
            // PlayerQuickAccess.CHARACTER_BODY.MoveAndSlideWithSnap(TotalMovement() + floorConnect, snap, Vector3.Up, true, 1);
        }

        // Really the only time I have to update the floor detection.  Just adding that walls are now floors lol
        public override void FloorDetection()
        {
            bool grounded = PlayerQuickAccess.CHARACTER_BODY.IsOnFloor();
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
