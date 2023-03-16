using Godot;
using System;

namespace Player.BodyMods
{
    public partial class MantleRaycasters : Node3D
    {
        private RayCast3D[] Raycasters { get; set; }

        private Vector3 distance = new Vector3();

        [Export]
        public RayCast3D RightMost { get; private set; }
        [Export]
        public RayCast3D LeftMost { get; private set; }

        public override void _Ready()
        {
            Raycasters = new RayCast3D[GetChildCount()];
            for (int i = 0; i < GetChildCount(); i++)
            {
                Raycasters[i] = GetChild<RayCast3D>(i);
            }
        }

        public void SetTargetPosition(Vector3 dis)
        {
            foreach (RayCast3D r in Raycasters)
            {
                r.TargetPosition = dis;
            }
        }

        public bool IsColliding()
        {
            for (int i = 0; i < Raycasters.Length; i++)
            {
                if (Raycasters[i].IsColliding())
                {
                    return true;
                }
            }
            return false;
        }

        public Vector3 FurthestHit()
        {
            Vector3 point = Raycasters[0].TargetPosition + Raycasters[0].GlobalPosition;
            float pointDistance = 0f;
            for (int i = 0; i < Raycasters.Length; i++)
            {
                if (Raycasters[i].IsColliding())
                {
                    float distance = Raycasters[i].GlobalPosition.DistanceSquaredTo(Raycasters[i].GetCollisionPoint());
                    if (pointDistance < distance)
                    {
                        point = Raycasters[i].GetCollisionPoint();
                        pointDistance = distance;
                    }
                }
            }
            return point;
        }

        public float GetSmallestDistance()
        {
            float distance = Mathf.Inf;
            for (int i = 0; i < Raycasters.Length; i++)
            {
                if (Raycasters[i].IsColliding())
                {
                    float dis = Raycasters[i].GlobalPosition.DistanceSquaredTo(Raycasters[i].GetCollisionPoint());
                    distance = distance > dis ? dis : distance;
                }
            }
            return Mathf.Sqrt(distance);
        }

        private RayCast3D GetClosestColliding()
        {
            int holder = 0;
            float distance = Mathf.Inf;
            for (int i = 0; i < Raycasters.Length; i++)
            {
                if (Raycasters[i].IsColliding())
                {
                    float dis = Raycasters[i].GlobalPosition.DistanceSquaredTo(Raycasters[i].GetCollisionPoint());
                    if (distance > dis)
                    {
                        distance = dis;
                        holder = i;
                    }

                }
            }
            return Raycasters[holder];
        }

        private RayCast3D GetFurestColliding()
        {
            int holder = 0;
            float distance = 0f;
            for (int i = 0; i < Raycasters.Length; i++)
            {
                if (Raycasters[i].IsColliding())
                {
                    float dis = Raycasters[i].GlobalPosition.DistanceSquaredTo(Raycasters[i].GetCollisionPoint());
                    if (distance < dis)
                    {
                        distance = dis;
                        holder = i;
                    }

                }
            }
            return Raycasters[holder];
        }

        private float DistanceToPoint(RayCast3D ray)
        {
            return ray.GlobalPosition.DistanceTo(ray.GetCollisionPoint());
        }

        public Vector3 GetCasterAngle(bool rightToLeft)
        {
            return (RightMost.GetCollisionPoint() - LeftMost.GetCollisionPoint()) * (rightToLeft ? 1 : -1);
        }

    }

}
