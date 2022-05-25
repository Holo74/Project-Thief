using Godot;
using System;
using Player;

namespace Player.Rotation
{
    public static class BasicRotation
    {
        private static float HeadRotation { get; set; } = 0f;
        private static float HeadMinXRotation = Mathf.Deg2Rad(-80f);
        private static float HeadMaxXRotation = Mathf.Deg2Rad(80f);

        public static void BaseRotate(InputEventMouseMotion e)
        {
            float yRotation = Mathf.Deg2Rad(-e.Relative.x) * Management.Game.Settings.MOUSE_ROTATION;
            float xRotation = Mathf.Deg2Rad(-e.Relative.y) * Management.Game.Settings.MOUSE_ROTATION;
            PlayerQuickAccess.BODY_ROTATION.RotateY(yRotation);
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

