using UnityEngine;

[CreateAssetMenu(fileName = "FirerateRare", menuName = "Upgrades/Universal/Firerate/FirerateRare")]
public class FirerateRare : Upgrade
{
    public float fireRateMultiplierIncrease;
    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        upgradeManager.playerFireRateMultiplier += upgradeManager.playerFireRateMultiplier / 10f * fireRateMultiplierIncrease;
        // Update weapon stats to reflect new multiplier
        upgradeManager.UpdateWeaponFireRate();
    }
}
