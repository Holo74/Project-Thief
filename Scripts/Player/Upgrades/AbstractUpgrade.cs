using Godot;
using System;

namespace Player.Upgrades
{
    public abstract class AbstractUpgrade
    {
        public enum ITEM_UPGRADE_LIST
        {
            DoubleJump = 0,
            WallRun = 1,
            Sliding = 2
        }
        private static AbstractUpgrade[] UPGRADE_LIST =
        {
            new DoubleJump(),
            new WallRun(),
            new Sliding()
        };

        public static AbstractUpgrade GetUpgrade(ITEM_UPGRADE_LIST position)
        {
            return UPGRADE_LIST[((int)position)];
        }

        public abstract void Applied();
        public abstract void Update(float delta);
        public abstract void Removed();
    }

}
