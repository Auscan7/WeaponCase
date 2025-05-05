using UnityEngine;

[CreateAssetMenu(fileName = "FirerateUnique", menuName = "Upgrades/Universal/Firerate/FirerateUnique")]
public class FirerateUnique : Upgrade
{
    public float fireRateMultiplierIncrease;
    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        upgradeManager.playerFireRateMultiplier += upgradeManager.playerFireRateMultiplier / 10f * fireRateMultiplierIncrease;
        // Update weapon stats to reflect new multiplier
        upgradeManager.UpdateWeaponFireRate();
    }
}
