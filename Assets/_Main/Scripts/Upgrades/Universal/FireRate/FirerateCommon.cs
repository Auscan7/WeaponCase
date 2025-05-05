using UnityEngine;

[CreateAssetMenu(fileName = "FirerateCommon", menuName = "Upgrades/Universal/Firerate/FirerateCommon")]
public class FirerateCommon : Upgrade
{
    public float fireRateMultiplierIncrease;
    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        upgradeManager.playerFireRateMultiplier += upgradeManager.playerFireRateMultiplier / 10f * fireRateMultiplierIncrease;
        // Update weapon stats to reflect new multiplier
        upgradeManager.UpdateWeaponFireRate();
    }
}

