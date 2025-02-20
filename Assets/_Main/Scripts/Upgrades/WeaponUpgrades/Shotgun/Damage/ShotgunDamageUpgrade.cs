using UnityEngine;
using static Upgrade;

[CreateAssetMenu(fileName = "ShotgunDamageUpgrade", menuName = "Upgrades/WeaponImprovement/Shotgun/ShotgunDamage")]
public class ShotgunDamageUpgrade : Upgrade, IConditionalUpgrade
{
    public float damageIncrease;

    public override void ApplyUpgrade(UpgradeManager upgradeManager)
    {
        upgradeManager.shotgunStats.damage += upgradeManager.shotgunStats.damage / 10 * damageIncrease;
    }

    public bool CanOffer()
    {
        return UpgradeManager.Instance.IsWeaponActive("Shotgun");
    }
}
