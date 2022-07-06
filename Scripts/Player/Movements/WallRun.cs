using Godot;
using System;

namespace Player.Movement
{
    public class WallRun : AbstractMovement
    {
        private float Buffer = 0;
        protected bool currentFloorState = false;
        public override void Movement(float delta)
        {
            // The snap direction is determined by whether you're running on the wall or the floor
            Vector3 snap = Vector3.Down;
            // Pushes the player onto a floor
            Vector3 floorConnect = Vector3.Down * 0.1f;
            Vector3 jumpFactor = Vector3.Zero;
            Variables.WALKING_MOVEMENT = DirectionalInput();
            // This function is only active when the player is "on floor" and with walls as floor now we can assume this case
            if (!PlayerQuickAccess.KINEMATIC_BODY.IsOnFloor())
            {
                if (PlayerQuickAccess.WALL_DETECTION.IsWallDetected(false))
                {
                    snap = PlayerQuickAccess.WALL_DETECTION.GetLeftNormal();
                    jumpFactor = PlayerQuickAccess.WALL_DETECTION.GetLeftNormal();
                    floorConnect = -snap;
                    Variables.WALKING_MOVEMENT = snap.Cross(Vector3.Down);
                }
                if (PlayerQuickAccess.WALL_DETECTION.IsWallDetected(true))
                {
                    snap = PlayerQuickAccess.WALL_DETECTION.GetRightNormal();
                    jumpFactor = PlayerQuickAccess.WALL_DETECTION.GetRightNormal();
                    floorConnect = -snap;
                    Variables.WALKING_MOVEMENT = snap.Cross(Vector3.Up);
                }
            }
            else
            {
                floorConnect = Vector3.Zero;
                snap = Vector3.Down;
            }
            Variables.WALKING_MOVEMENT *= MovementSpeed();
            if (Input.IsActionJustPressed("ui_select"))
            {
                Jump(jumpFactor * Variables.JUMP_STRENGTH);
            }
            if (Variables.GRAVITY_MOVEMENT.y > 0)
            {
                snap = Vector3.Zero;
            }
            PlayerQuickAccess.KINEMATIC_BODY.MoveAndSlideWithSnap(TotalMovement() + floorConnect, snap, Vector3.Up, true, 1);
        }

        // Really the only time I have to update the floor detection.  Just adding that walls are now floors lol
        public override void FloorDetection()
        {
            bool grounded = PlayerQuickAccess.KINEMATIC_BODY.IsOnFloor();
            // Walls are only floors if you are moving fast
            bool canRun = PlayerQuickAccess.WALL_DETECTION.IsWallDetected(true) || PlayerQuickAccess.WALL_DETECTION.IsWallDetected(false);
            canRun = Variables.WALKING_MOVEMENT.LengthSquared() > 1f ? canRun : false;
            grounded = grounded || (canRun);
            if (grounded != currentFloorState)
            {
                Variables.ON_FLOOR = grounded;
                currentFloorState = Variables.ON_FLOOR;
            }
        }
    }
}
