using Godot;
using System;

namespace Player.BodyMods
{
    public class MantleRaycasters : Spatial
    {
        private RayCast[] Raycasters { get; set; }

        private Vector3 distance = new Vector3();

        public override void _Ready()
        {
            Raycasters = new RayCast[GetChildCount()];
            for (int i = 0; i < GetChildCount(); i++)
            {
                Raycasters[i] = GetChild<RayCast>(i);
            }
        }

        public void SetCastTo(Vector3 dis)
        {
            foreach (RayCast r in Raycasters)
            {
                r.CastTo = dis;
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
            Vector3 point = Raycasters[0].CastTo + Raycasters[0].GlobalTranslation;
            float pointDistance = 0f;
            for (int i = 0; i < Raycasters.Length; i++)
            {
                if (Raycasters[i].IsColliding())
                {
                    float distance = Raycasters[i].GlobalTranslation.DistanceSquaredTo(Raycasters[i].GetCollisionPoint());
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
                    float dis = Raycasters[i].GlobalTranslation.DistanceSquaredTo(Raycasters[i].GetCollisionPoint());
                    distance = distance > dis ? dis : distance;
                }
            }
            return Mathf.Sqrt(distance);
        }

    }

}
