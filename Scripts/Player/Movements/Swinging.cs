using Godot;
using System;

namespace Player.Movement
{
    // This needs a lot of work.  I wouldn't recommeend using it for now
    public class Swinging : AbstractMovement
    {
        private Vector3 AnchorPoint { get; set; }
        private Vector3 Right { get; set; }
        private float PreviousDot { get; set; }
        private float Speed { get; set; }
        private float MaxSwingAngle { get; set; }
        private float MaxDistance { get; set; }

        private Vector3 ForwardMove()
        {
            Vector3 directionToAnchor = DirectionToAnchor();
            directionToAnchor = directionToAnchor.Normalized();
            return directionToAnchor.Cross(Right);
        }

        private Vector3 DirectionToAnchor()
        {
            return AnchorPoint - Player.PlayerQuickAccess.KINEMATIC_BODY.GlobalTransform.origin;
        }

        public Swinging(Vector3 anchor, Vector3 right)
        {
            AnchorPoint = anchor;
            Right = right;
            Speed = Variables.WALKING_MOVEMENT.Length() + Variables.GRAVITY_MOVEMENT.Length();
            Vector3 forward = Vector3.Up.Cross(Right);
            Vector3 endPoint = DirectionToAnchor();
            endPoint.y = 0;
            endPoint = endPoint.Normalized();
            Right *= (endPoint.Dot(forward) > 0) ? 1 : -1;
            MaxSwingAngle = Mathf.Clamp(1f - (Speed / 10.0f), -.8f, 1f);
            MaxDistance = Player.PlayerQuickAccess.KINEMATIC_BODY.GlobalTransform.origin.DistanceTo(anchor);
        }

        public override void FallingMovement(float delta)
        {
            Swing(delta);
        }

        public override void Movement(float delta)
        {
            Swing(delta);
        }

        private void Swing(float delta)
        {
            float dot = Vector3.Up.Dot((DirectionToAnchor()).Normalized());
            if (dot < MaxSwingAngle)
            {
                if (dot < PreviousDot)
                {
                    Right = -Right;
                }
            }
            Vector3 movingTo = ForwardMove();
            float distance = DirectionToAnchor().Length() - MaxDistance;
            if (distance > 0)
            {
                movingTo += DirectionToAnchor() * distance;
            }
            if (!(PlayerQuickAccess.KINEMATIC_BODY.MoveAndCollide(movingTo * delta * Speed * (dot - MaxSwingAngle + .1f)) is null))
            {
                Right = -Right;
            }
            PreviousDot = dot;
            if (Input.IsActionJustPressed("ui_select"))
            {
                Jump(movingTo * Speed * (dot - MaxSwingAngle + .1f));
                Variables.RESET_MOVEMENT();
            }
        }

        public override void FloorDetection()
        {

        }

        public override void Starting()
        {
            Variables.ON_FLOOR = false;
        }
    }

}
