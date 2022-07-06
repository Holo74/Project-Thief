using Godot;
using System;
using Player;

namespace Player.Rotation
{
    public class BasicRotation
    {
        private float HeadRotation { get; set; } = 0f;
        private float HeadMinXRotation = Mathf.Deg2Rad(-80f);
        private float HeadMaxXRotation = Mathf.Deg2Rad(80f);

        public BasicRotation()
        {
            Vector3 forward = -PlayerQuickAccess.CAMERA.GlobalTransform.basis.z;
            Vector3 horizontal = forward;
            horizontal.y = 0f;
            horizontal = horizontal.Normalized();
            float angle = horizontal.SignedAngleTo(forward, PlayerQuickAccess.CAMERA.GlobalTransform.basis.x);
            HeadRotation = angle;
        }

        // Use this to pass the amount of rotation and call horizontal and vertical rotations
        public void BaseRotate(InputEventMouseMotion e)
        {
            float yRotation = Mathf.Deg2Rad(-e.Relative.x) * Management.Game.Settings.MOUSE_ROTATION;
            float xRotation = Mathf.Deg2Rad(-e.Relative.y) * Management.Game.Settings.MOUSE_ROTATION;
            HorizontalRotation(yRotation);
            VerticalRotation(xRotation);
        }

        protected virtual void HorizontalRotation(float yRotation)
        {
            PlayerQuickAccess.BODY_ROTATION.RotateY(yRotation);
        }

        protected virtual void VerticalRotation(float xRotation)
        {
            float subtraction = 0f;
            if (xRotation > 0)
            {
                subtraction = Mathf.Clamp(HeadMaxXRotation - HeadRotation - xRotation, -HeadMaxXRotation, 0f);
            }
            else
            {
                subtraction = Mathf.Clamp(HeadMinXRotation - HeadRotation - xRotation, 0f, -HeadMinXRotation);
            }

            xRotation += subtraction;
            HeadRotation += xRotation;
            PlayerQuickAccess.CAMERA.RotateX(xRotation);
        }
    }
}

