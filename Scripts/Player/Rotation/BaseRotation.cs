using Godot;
using System;
using Player;

namespace Player.Rotation
{
    public partial class BasicRotation
    {
        private float HeadRotation { get; set; } = 0f;
        private float HeadMinXRotation = Mathf.DegToRad(-80f);
        private float HeadMaxXRotation = Mathf.DegToRad(80f);

        public BasicRotation()
        {
            Vector3 forward = -PlayerQuickAccess.CAMERA.GlobalTransform.Basis.Z;
            Vector3 horizontal = forward;
            horizontal.Y = 0f;
            horizontal = horizontal.Normalized();
            float angle = horizontal.SignedAngleTo(forward, PlayerQuickAccess.CAMERA.GlobalTransform.Basis.X);
            HeadRotation = angle;
        }

        // Use this to pass the amount of rotation and call horizontal and vertical rotations
        public void BaseRotate(InputEventMouseMotion e)
        {
            float yRotation = Mathf.DegToRad(-e.Relative.X) * Management.Game.Settings.MOUSE_ROTATION;
            float xRotation = Mathf.DegToRad(-e.Relative.Y) * Management.Game.Settings.MOUSE_ROTATION;
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
            PlayerQuickAccess.CAMERA.Rotate(PlayerQuickAccess.CAMERA.Basis.X, xRotation);
        }
    }
}

