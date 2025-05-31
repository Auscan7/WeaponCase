using UnityEngine;

[CreateAssetMenu(fileName = "RamDamageUpgrade", menuName = "Upgrades/WeaponImprovement/RamDamageUpgrade")]
public class RamDamageUpgrade : Upgrade
{
    public float damageIncrease;
    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        upgradeManager.boatColliderDamageMultiplier += damageIncrease;
        upgradeManager.UpdateWeaponDamage();
    }
}
