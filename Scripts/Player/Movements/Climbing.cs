using Godot;
using System;

namespace Player.Movement
{
    public class Climbing : AbstractMovement
    {
        public Climbing(Vector3 right)
        {
            Right = right;
        }
        private Vector3 Right { get; set; }
        private Vector3 Forward { get; set; }
        private bool TouchingFloor { get; set; } = true;

        public override void FallingMovement(float delta)
        {
            Move();
            TouchingFloor = false;
        }

        public override void FloorDetection()
        {
            if (Variables.ON_FLOOR != PlayerQuickAccess.FLOOR_CAST.IsColliding())
            {
                Variables.ON_FLOOR = PlayerQuickAccess.FLOOR_CAST.IsColliding();
            }
        }

        public override void Movement(float delta)
        {
            Move();
            if (!TouchingFloor)
            {
                Variables.RESET_MOVEMENT();
            }
        }

        private Vector3 Movement()
        {
            Vector3 output = new Vector3();
            output = (InputToInt("ui_down") - InputToInt("ui_up")) * Vector3.Down;
            output += (InputToInt("ui_right") - InputToInt("ui_left")) * Right;
            return output.Normalized();
        }

        private void Move()
        {
            Variables.WALKING_MOVEMENT = Movement() + Forward;
            PlayerQuickAccess.KINEMATIC_BODY.MoveAndSlide(Variables.WALKING_MOVEMENT * Variables.SPEED);
            if (Input.IsActionJustPressed("ui_select"))
            {
                Jump(Vector3.Down.Cross(Right) * Variables.SPEED);
                Variables.RESET_MOVEMENT();
            }
        }

        public override void Starting()
        {
            Forward = Vector3.Up.Cross(Right);
        }
    }

}
