using UnityEngine;
using static Upgrade;

[CreateAssetMenu(fileName = "SlingShotActivateUpgrade", menuName = "Upgrades/WeaponUnlocks/SlingShot")]
public class ActivateSlingShotUpgrade : Upgrade, IConditionalUpgrade
{
    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        upgradeManager.ActivateWeapon("SlingShot");
    }
    public bool CanOffer()
    {
        // Only offer if the sling shot is not active yet.
        // Only offer if the player selected the wooden boat.
        // Assuming your UpgradeManager stores a weaponDict.
        return !PlayerUpgradeManager.Instance.IsWeaponActive("SlingShot") &&
               PlayerUpgradeManager.Instance.SelectedBoatName == "Raft" &&
           PlayerUpgradeManager.Instance.HasFreeWeaponSlot();
    }
}
