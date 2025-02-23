using UnityEngine;
using static Upgrade;

[CreateAssetMenu(fileName = "PistolFireRateUpgrade", menuName = "Upgrades/WeaponImprovement/Pistol/PistolFireRate")]
public class PistolFireRateUpgrade : Upgrade, IConditionalUpgrade
{
    public float fireRateIncrease;

    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        // Increase pistol's fire rate.
        upgradeManager.pistolStats.firerate +=  upgradeManager.pistolStats.firerate / 10 * fireRateIncrease;
    }

    public bool CanOffer()
    {
        // Only offer if the pistol is active.
        return PlayerUpgradeManager.Instance.IsWeaponActive("Pistol");
    }
}
