using UnityEngine;
using static Upgrade;

[CreateAssetMenu(fileName = "SpearRangeUpgrade", menuName = "Upgrades/WeaponImprovement/Spear/SpearRange")]
public class SpearRange : Upgrade, IConditionalUpgrade
{
    public float rangeIncrease;

    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        upgradeManager.spearStats.range += rangeIncrease;
    }

    public bool CanOffer()
    {
        // Only offer if the flame circle is active.
        return PlayerUpgradeManager.Instance.IsWeaponActive("Spear");
    }

}
