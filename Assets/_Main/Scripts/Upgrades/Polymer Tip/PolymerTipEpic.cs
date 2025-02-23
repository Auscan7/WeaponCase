using UnityEngine;

[CreateAssetMenu(fileName = "PolymerTipEpic", menuName = "Upgrades/PolymerTip/PolymerTipEpic")]
public class PolymerTipEpic : Upgrade
{
    public float damageMultiplierIncrease;

    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        upgradeManager.playerDamageMultiplier += upgradeManager.playerDamageMultiplier / 10f * damageMultiplierIncrease;

        // Update weapon stats to reflect new multiplier
        upgradeManager.UpdateWeaponDamage();
    }
}
