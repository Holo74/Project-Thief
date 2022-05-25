using Godot;
using System;

namespace Player.Upgrades
{
    public class WallRun : AbstractUpgrade
    {
        public override void Applied()
        {
            Variables.DEFAULT_MOVEMENT = new Movement.WallRun();
        }

        public override void Update(float delta)
        {
        }

        public override void Removed()
        {
        }
    }

}
