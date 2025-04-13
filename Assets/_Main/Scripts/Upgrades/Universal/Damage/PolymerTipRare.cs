using UnityEngine;

[CreateAssetMenu(fileName = "PolymerTipRare", menuName = "Upgrades/Universal/PolymerTip/PolymerTipRare")]
public class PolymerTipRare : Upgrade
{
    public float damageMultiplierIncrease;

    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        upgradeManager.playerDamageMultiplier += upgradeManager.playerDamageMultiplier / 10f * damageMultiplierIncrease;

        // Update weapon stats to reflect new multiplier
        upgradeManager.UpdateWeaponDamage();
    }
}
