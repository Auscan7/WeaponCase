using UnityEngine;

[CreateAssetMenu(fileName = "FirerateEpic", menuName = "Upgrades/Universal/Firerate/FirerateEpic")]
public class FirerateEpic : Upgrade
{
    public float fireRateMultiplierIncrease;
    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        upgradeManager.playerFireRateMultiplier += upgradeManager.playerFireRateMultiplier / 10f * fireRateMultiplierIncrease;
        // Update weapon stats to reflect new multiplier
        upgradeManager.UpdateWeaponFireRate();
    }
}
