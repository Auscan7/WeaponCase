using UnityEngine;
using static Upgrade;

[CreateAssetMenu(fileName = "PistolDamageUpgrade", menuName = "Upgrades/WeaponImprovement/Pistol/PistolDamage")]
public class PistolDamageUpgrade : Upgrade, IConditionalUpgrade
{
    public float damageIncrease;

    public override void ApplyUpgrade(UpgradeManager upgradeManager)
    {
        // Increase pistol's fire rate.
        upgradeManager.pistolStats.damage += upgradeManager.pistolStats.damage / 10 * damageIncrease;
    }

    public bool CanOffer()
    {
        // Only offer if the pistol is active.
        return UpgradeManager.Instance.IsWeaponActive("Pistol");
    }
}
