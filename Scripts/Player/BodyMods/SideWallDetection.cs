using Godot;
using System;

namespace Player.BodyMods
{
    public partial class SideWallDetection : Node
    {
        private RayCast3D RightRay { get; set; }
        private RayCast3D LeftRay { get; set; }

        public override void _Ready()
        {
            LeftRay = GetNode<RayCast3D>("Left Ray");
            RightRay = GetNode<RayCast3D>("Right Ray");
            LeftRay.AddException(GetParent<CharacterBody3D>());
            RightRay.AddException(GetParent<CharacterBody3D>());
        }

        public Vector3 GetRightNormal()
        {
            if (RightRay.IsColliding())
            {
                return RightRay.GetCollisionNormal();
            }
            return -PlayerQuickAccess.BODY_DIRECTION.X;
        }

        public Vector3 GetLeftNormal()
        {
            if (LeftRay.IsColliding())
            {
                return LeftRay.GetCollisionNormal();
            }
            return PlayerQuickAccess.BODY_DIRECTION.X;
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
