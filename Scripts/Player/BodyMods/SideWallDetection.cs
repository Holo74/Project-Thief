using Godot;
using System;

namespace Player.BodyMods
{
    public class SideWallDetection : Node
    {
        private RayCast RightRay { get; set; }
        private RayCast LeftRay { get; set; }

        public override void _Ready()
        {
            LeftRay = GetNode<RayCast>("Left Ray");
            RightRay = GetNode<RayCast>("Right Ray");
        }

        public Vector3 GetRightNormal()
        {
            if (RightRay.IsColliding())
            {
                return RightRay.GetCollisionNormal();
            }
            return -PlayerQuickAccess.BODY_DIRECTION.x;
        }

        public Vector3 GetLeftNormal()
        {
            if (LeftRay.IsColliding())
            {
                return LeftRay.GetCollisionNormal();
            }
            return PlayerQuickAccess.BODY_DIRECTION.x;
        }

        public bool IsWallDetected(bool isRight)
        {
            if (isRight)
            {
                return RightRay.IsColliding();
            }
            else
            {
                return LeftRay.IsColliding();
            }
        }
    }
}
