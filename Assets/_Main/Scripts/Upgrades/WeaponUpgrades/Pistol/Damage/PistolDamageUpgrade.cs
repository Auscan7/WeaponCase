using UnityEngine;
using static Upgrade;

[CreateAssetMenu(fileName = "PistolDamageUpgrade", menuName = "Upgrades/WeaponImprovement/Pistol/PistolDamage")]
public class PistolDamageUpgrade : Upgrade, IConditionalUpgrade
{
    public float damageIncrease;

    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        upgradeManager.pistolDamageMultiplier += damageIncrease;
        upgradeManager.UpdateWeaponDamage();
    }

    public bool CanOffer()
    {
        // Only offer if the pistol is active.
        return PlayerUpgradeManager.Instance.IsWeaponActive("Pistol");
    }
}
