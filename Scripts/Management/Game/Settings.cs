using Godot;
using System;

namespace Management.Game
{
    public static class Settings
    {
        public static float MOUSE_ROTATION
        {
            get { return MouseRotation; }
            set { MouseRotation = value / 100f; }
        }
        private static float MouseRotation = 0.2f;
    }
}

