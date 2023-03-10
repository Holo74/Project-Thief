using Godot;
using System;

namespace Player.Upgrades
{
    // Applies the upgrade wall run and then resets the movement so that the player uses it from the moment they get it
    public class WallRun : AbstractUpgrade
    {
        public override void Applied()
        {
            Variables.Instance.DEFAULT_MOVEMENT = new Movement.WallRun();
        }

        public override void Update(float delta)
        {
        }

        public override void Removed()
        {
        }
    }

}
