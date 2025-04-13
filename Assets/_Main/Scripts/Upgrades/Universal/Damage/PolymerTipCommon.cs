using UnityEngine;

[CreateAssetMenu(fileName = "PolymerTipCommon", menuName = "Upgrades/Universal/PolymerTip/PolymerTipCommon")]
public class PolymerTipCommon : Upgrade
{
    public float damageMultiplierIncrease;

    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        upgradeManager.playerDamageMultiplier += upgradeManager.playerDamageMultiplier / 10f * damageMultiplierIncrease;

        // Update weapon stats to reflect new multiplier
        upgradeManager.UpdateWeaponDamage();
    }
}
