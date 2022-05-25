using Godot;
using System;

namespace Player.Movement
{
    public class Sliding : AbstractMovement
    {
        private float Modifier { get; set; }
        public Sliding(float modifier)
        {
            Modifier = modifier;
        }

        public override void Movement(float delta)
        {
            if (Input.IsActionJustPressed("ui_select"))
            {
                Jump(Variables.WALKING_MOVEMENT * Modifier);
                FallingMovement(delta);
                Variables.RESET_MOVEMENT();
                return;
            }
            PlayerQuickAccess.KINEMATIC_BODY.MoveAndSlideWithSnap(TotalMovement() * Modifier, Vector3.Down, Vector3.Up);
            if (Variables.WALKING_MOVEMENT.Length() < Variables.CROUCH_SPEED)
            {
                Variables.RESET_MOVEMENT();
            }
            Variables.WALKING_MOVEMENT *= .99f;
        }
    }

}
