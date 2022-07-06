using Godot;
using System;

namespace Player.Handlers
{
    public class UpgradeHandler : Resource
    {
        public void Init()
        {
            UpgradeList = new System.Collections.Generic.List<Upgrades.AbstractUpgrade>();
            foreach (Upgrades.AbstractUpgrade upgrade in UpgradeList)
            {
                upgrade.Applied();
            }
        }
        [Export]
        public System.Collections.Generic.List<Upgrades.AbstractUpgrade> UpgradeList { get; private set; } = new System.Collections.Generic.List<Upgrades.AbstractUpgrade>();

        public void AddUpgrade(Upgrades.AbstractUpgrade upgrade)
        {
            upgrade.Applied();
            UpgradeList.Add(upgrade);
        }

        public void RemoveUpgrade(Upgrades.AbstractUpgrade upgrade)
        {
            if (UpgradeList.Contains(upgrade))
            {
                UpgradeList.Remove(upgrade);
                upgrade.Removed();
            }
        }

        public void RunUpgrades(float delta)
        {
            foreach (Upgrades.AbstractUpgrade upgrade in UpgradeList)
            {
                upgrade.Update(delta);
            }
        }
    }
}
