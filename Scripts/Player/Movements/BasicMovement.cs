using Godot;
using System;

namespace Player.Movement
{
    public class BasicMovement : AbstractMovement
    {
        public override void Movement(float delta)
        {
            if (Input.IsActionJustPressed("ui_select"))
            {
                Jump(Vector3.Zero);
                FallingMovement(delta);
                return;
            }
            Variables.WALKING_MOVEMENT = DirectionalInput() * MovementSpeed();
            Variables.MOVEMENT.Crouch();
            Variables.MOVEMENT.Crawl();
            Vector3 floorConnect = PlayerQuickAccess.KINEMATIC_BODY.IsOnFloor() ? Vector3.Zero : Vector3.Down * 0.1f;
            PlayerQuickAccess.KINEMATIC_BODY.MoveAndSlideWithSnap(TotalMovement() + floorConnect, Vector3.Down * .1f, Vector3.Up, true, 1);
        }
    }

}
