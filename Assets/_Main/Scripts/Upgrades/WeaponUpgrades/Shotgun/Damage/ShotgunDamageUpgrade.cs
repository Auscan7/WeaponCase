using UnityEngine;
using static Upgrade;

[CreateAssetMenu(fileName = "ShotgunDamageUpgrade", menuName = "Upgrades/WeaponImprovement/Shotgun/ShotgunDamage")]
public class ShotgunDamageUpgrade : Upgrade, IConditionalUpgrade
{
    public float damageIncrease;

    public override void ApplyUpgrade(UpgradeManager upgradeManager)
    {
        upgradeManager.shotgunDamageMultiplier += damageIncrease / 10f;
        upgradeManager.UpdateWeaponDamage();
    }

    public bool CanOffer()
    {
        return UpgradeManager.Instance.IsWeaponActive("Shotgun");
    }
}
