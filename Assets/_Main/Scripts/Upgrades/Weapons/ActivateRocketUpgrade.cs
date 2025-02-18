using UnityEngine;
using static Upgrade;

[CreateAssetMenu(fileName = "RocketActivateUpgrade", menuName = "Upgrades/WeaponUnlocks/Rocket")]
public class ActivateRocketUpgrade : Upgrade, IConditionalUpgrade
{
    public override void ApplyUpgrade(UpgradeManager upgradeManager)
    {
        upgradeManager.ActivateWeapon("Rocket");
    }

    public bool CanOffer()
    {
        // Only offer if the shotgun is not active yet.
        // Assuming your UpgradeManager stores a weaponDict.
        return !UpgradeManager.Instance.IsWeaponActive("Rocket");
    }
}
