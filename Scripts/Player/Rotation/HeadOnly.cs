using Godot;
using System;

namespace Player.Rotation
{
    public partial class HeadOnly : BasicRotation
    {
        protected override void HorizontalRotation(float yRotation)
        {
            PlayerQuickAccess.CAMERA.RotateY(yRotation);
        }
    }

}
