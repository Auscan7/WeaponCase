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
        // Only offer if the pistol is not active yet.
        // Only offer if the player selected the battle ship boat.
        // Assuming your UpgradeManager stores a weaponDict.
        return !PlayerUpgradeManager.Instance.IsWeaponActive("Pistol") &&
               PlayerUpgradeManager.Instance.SelectedBoatName == "Battle Ship" &&
           PlayerUpgradeManager.Instance.HasFreeWeaponSlot();
    }
}
