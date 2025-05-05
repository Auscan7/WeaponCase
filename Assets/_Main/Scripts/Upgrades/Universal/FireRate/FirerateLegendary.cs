using UnityEngine;

[CreateAssetMenu(fileName = "FirerateLegendary", menuName = "Upgrades/Universal/Firerate/FirerateLegendary")]
public class FirerateLegendary : Upgrade
{
    public float fireRateMultiplierIncrease;
    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        upgradeManager.playerFireRateMultiplier += upgradeManager.playerFireRateMultiplier / 10f * fireRateMultiplierIncrease;
        // Update weapon stats to reflect new multiplier
        upgradeManager.UpdateWeaponFireRate();
    }
}
