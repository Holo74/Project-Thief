using Godot;
using System;

namespace Player.Handlers
{
    public partial class UpgradeHandler : Resource
    {
        public UpgradeHandler()
        {

        }
        public void Init()
        {
            UpgradeList = new Godot.Collections.Array<Upgrades.AbstractUpgrade>();
            foreach (Upgrades.AbstractUpgrade upgrade in UpgradeList)
            {
                upgrade.Applied();
            }
        }
        [Export]
        public Godot.Collections.Array<Upgrades.AbstractUpgrade> UpgradeList { get; private set; } = new Godot.Collections.Array<Upgrades.AbstractUpgrade>();

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

        public void RunUpgrades(double delta)
        {
            foreach (Upgrades.AbstractUpgrade upgrade in UpgradeList)
            {
                upgrade.Update(delta);
            }
        }
    }
}
