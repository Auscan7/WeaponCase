using UnityEngine;
using static Upgrade;

[CreateAssetMenu(fileName = "BowAndArrowFirerateUpgrade", menuName = "Upgrades/WeaponImprovement/BowAndArrow/BowAndArrowFirerate")]
public class BowAndArrowFirerateUpgrade : Upgrade, IConditionalUpgrade
{
    public float fireRateIncrease;

    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        upgradeManager.bowAndArrowStats.firerate += upgradeManager.bowAndArrowStats.firerate / 10 * fireRateIncrease;
    }

    public bool CanOffer()
    {
        // Only offer if the bow and arrow is active.
        return PlayerUpgradeManager.Instance.IsWeaponActive("BowAndArrow");
    }
}
