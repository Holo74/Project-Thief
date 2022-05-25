using Godot;
using System;

namespace Player.Movement
{
    public class Mantle : AbstractMovement
    {
        public override void Starting()
        {
            Variables.ON_FLOOR = false;
        }

        public override void FloorDetection()
        {

        }

        public override void Movement(float delta)
        {
            MantleUp();
        }

        public override void FallingMovement(float delta)
        {
            MantleUp();
        }

        private void MantleUp()
        {
            // PhysicsServer.
        }
    }

}
