using UnityEngine;
using static Upgrade;

[CreateAssetMenu(fileName = "PistolActivateUpgrade", menuName = "Upgrades/WeaponUnlocks/Pistol")]
public class ActivatePistolUpgrade : Upgrade, IConditionalUpgrade
{
    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        upgradeManager.ActivateWeapon("Pistol");
    }
    public bool CanOffer()
    {
        // Only offer if the shotgun is not active yet.
        // Assuming your UpgradeManager stores a weaponDict.
        return !PlayerUpgradeManager.Instance.IsWeaponActive("Pistol");
    }
}
