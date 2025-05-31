using UnityEngine;
using static Upgrade;

[CreateAssetMenu(fileName = "ShotgunDamageUpgrade", menuName = "Upgrades/WeaponImprovement/Shotgun/ShotgunDamage")]
public class ShotgunDamageUpgrade : Upgrade, IConditionalUpgrade
{
    public float damageIncrease;

    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        upgradeManager.shotgunDamageMultiplier += damageIncrease;
        upgradeManager.UpdateWeaponDamage();
    }

    public bool CanOffer()
    {
        return PlayerUpgradeManager.Instance.IsWeaponActive("Shotgun");
    }
}
