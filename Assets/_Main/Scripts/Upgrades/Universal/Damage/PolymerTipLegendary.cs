using UnityEngine;

[CreateAssetMenu(fileName = "PolymerTipLegendary", menuName = "Upgrades/PolymerTip/PolymerTipLegendary")]
public class PolymerTipLegendary : Upgrade
{
    public float damageMultiplierIncrease;

    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        upgradeManager.playerDamageMultiplier += upgradeManager.playerDamageMultiplier / 10f * damageMultiplierIncrease;

        // Update weapon stats to reflect new multiplier
        upgradeManager.UpdateWeaponDamage();
    }
}
