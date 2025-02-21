using UnityEngine;
using static Upgrade;

[CreateAssetMenu(fileName = "PistolDamageUpgrade", menuName = "Upgrades/WeaponImprovement/Pistol/PistolDamage")]
public class PistolDamageUpgrade : Upgrade, IConditionalUpgrade
{
    public float damageIncrease;

    public override void ApplyUpgrade(UpgradeManager upgradeManager)
    {
        upgradeManager.pistolDamageMultiplier += damageIncrease / 10f;
        upgradeManager.UpdateWeaponDamage();
    }

    public bool CanOffer()
    {
        // Only offer if the pistol is active.
        return UpgradeManager.Instance.IsWeaponActive("Pistol");
    }
}
