using Godot;
using System;

namespace Player.Movement
{
    public partial class Climbing : AbstractMovement
    {
        public Climbing(Vector3 right)
        {
            Right = right;
        }
        private Vector3 Right { get; set; }
        private Vector3 Forward { get; set; }
        private bool TouchingFloor { get; set; } = true;

        public override void FallingMovement(double delta)
        {
            Move();
            TouchingFloor = false;
        }

        public override void FloorDetection()
        {
            if (Variables.Instance.ON_FLOOR != PlayerQuickAccess.FLOOR_CAST.IsColliding())
            {
                Variables.Instance.ON_FLOOR = PlayerQuickAccess.FLOOR_CAST.IsColliding();
            }
        }

        public override void Movement(double delta)
        {
            Move();
            if (!TouchingFloor)
            {
                Variables.Instance.RESET_MOVEMENT();
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
            Variables.Instance.WALKING_MOVEMENT = Movement() + Forward;
            PlayerQuickAccess.KINEMATIC_BODY.Velocity = Variables.Instance.WALKING_MOVEMENT * Variables.Instance.STANDING_SPEED;
            PlayerQuickAccess.KINEMATIC_BODY.MoveAndSlide();
            if (Input.IsActionJustPressed("ui_select"))
            {
                Jump(Vector3.Down.Cross(Right) * Variables.Instance.STANDING_SPEED);
                Variables.Instance.RESET_MOVEMENT();
            }
        }

        public override void Starting()
        {
            Forward = Vector3.Up.Cross(Right);
            PlayerQuickAccess.KINEMATIC_BODY.MotionMode = CharacterBody3D.MotionModeEnum.Grounded;
            PlayerQuickAccess.KINEMATIC_BODY.UpDirection = -Forward;
        }
    }

}
