using UnityEngine;

[CreateAssetMenu(fileName = "PolymerTipUnique", menuName = "Upgrades/Universal/PolymerTip/PolymerTipUnique")]
public class PolymerTipUnique : Upgrade
{
    public float damageMultiplierIncrease;

    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        upgradeManager.playerDamageMultiplier += upgradeManager.playerDamageMultiplier / 10f * damageMultiplierIncrease;

        // Update weapon stats to reflect new multiplier
        upgradeManager.UpdateWeaponDamage();
    }
}
