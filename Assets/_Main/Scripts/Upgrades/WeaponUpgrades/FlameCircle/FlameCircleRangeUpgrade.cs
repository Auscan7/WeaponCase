using UnityEngine;
using static Upgrade;

[CreateAssetMenu(fileName = "FlameCircleRangeUpgrade", menuName = "Upgrades/WeaponImprovement/FlameCircle/FlameCircleRange")]
public class FlameCircleRangeUpgrade : Upgrade, IConditionalUpgrade
{
    public float rangeIncrease;

    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        upgradeManager.flameCircleStats.range += rangeIncrease;
    }

    public bool CanOffer()
    {
        // Only offer if the flame circle is active.
        return PlayerUpgradeManager.Instance.IsWeaponActive("FlameCircle");
    }
}
