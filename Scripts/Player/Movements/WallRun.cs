using Godot;
using System;

namespace Player.Movement
{
    public class WallRun : AbstractMovement
    {
        private float Buffer = 0;
        public override void Movement(float delta)
        {

            Vector3 snap = Vector3.Down;
            Vector3 floorConnect = Vector3.Down * 0.1f;
            Vector3 jumpFactor = Vector3.Zero;
            Variables.WALKING_MOVEMENT = DirectionalInput();
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
            Variables.MOVEMENT.Crouch();
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

        public override void FloorDetection()
        {
            bool grounded = PlayerQuickAccess.KINEMATIC_BODY.IsOnFloor();
            bool canRun = PlayerQuickAccess.WALL_DETECTION.IsWallDetected(true) || PlayerQuickAccess.WALL_DETECTION.IsWallDetected(false);
            canRun = Variables.WALKING_MOVEMENT.LengthSquared() > 1f ? canRun : false;
            grounded = grounded || (canRun);
            if (grounded != currentFloorState)
            {
                Variables.ON_FLOOR = grounded;
                currentFloorState = Variables.ON_FLOOR;
            }
        }

        private bool BetweenValues(float value, float min, float max)
        {
            return (value > min) && (value < max);
        }
    }
}
