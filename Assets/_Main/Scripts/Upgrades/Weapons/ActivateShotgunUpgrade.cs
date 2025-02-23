using UnityEngine;
using static Upgrade;

[CreateAssetMenu(fileName = "ShotgunActivateUpgrade", menuName = "Upgrades/WeaponUnlocks/Shotgun")]
public class ActivateShotgunUpgrade : Upgrade, IConditionalUpgrade
{
    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        upgradeManager.ActivateWeapon("Shotgun");
    }
    public bool CanOffer()
    {
        // Only offer if the shotgun is not active yet.
        // Assuming your UpgradeManager stores a weaponDict.
        return !PlayerUpgradeManager.Instance.IsWeaponActive("Shotgun");
    }
}