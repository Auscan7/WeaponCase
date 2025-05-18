using UnityEngine;

[CreateAssetMenu(fileName = "DamageUpgrade", menuName = "Upgrades/Universal/DamageUpgrade")]
public class DamageUpgrade : Upgrade
{
    public float damageMultiplierIncrease;

    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        upgradeManager.playerDamageMultiplier += damageMultiplierIncrease;

        // Update weapon stats to reflect new multiplier
        upgradeManager.UpdateWeaponDamage();
    }
}
