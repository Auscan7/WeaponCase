using UnityEngine;

[CreateAssetMenu(fileName = "PolymerTipRare", menuName = "Upgrades/PolymerTip/PolymerTipRare")]
public class PolymerTipRare : Upgrade
{
    public float damageMultiplierIncrease;

    public override void ApplyUpgrade(UpgradeManager upgradeManager)
    {
        upgradeManager.playerDamageMultiplier += upgradeManager.playerDamageMultiplier / 10f * damageMultiplierIncrease;

        // Update weapon stats to reflect new multiplier
        upgradeManager.UpdateWeaponDamage();
    }
}
