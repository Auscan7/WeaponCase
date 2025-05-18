using UnityEngine;

[CreateAssetMenu(fileName = "FirerateUpgrade", menuName = "Upgrades/Universal/FirerateUpgrade")]
public class FirerateUpgrade : Upgrade
{
    public float fireRateMultiplierIncrease;
    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        upgradeManager.playerFireRateMultiplier += fireRateMultiplierIncrease;

        // Update weapon stats to reflect new multiplier
        upgradeManager.UpdateWeaponFireRate();
    }
}

