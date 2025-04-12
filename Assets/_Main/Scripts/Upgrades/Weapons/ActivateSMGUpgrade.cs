using UnityEngine;
using static Upgrade;

[CreateAssetMenu(fileName = "SMGActivateUpgrade", menuName = "Upgrades/WeaponUnlocks/SMG")]
public class ActivateSMGUpgrade : Upgrade, IConditionalUpgrade
{
    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        upgradeManager.ActivateWeapon("SMG");
    }
    public bool CanOffer()
    {
        // Only offer if the SMG is not active yet.
        // Only offer if the player selected the battle ship boat.
        // Assuming your UpgradeManager stores a weaponDict.
        return !PlayerUpgradeManager.Instance.IsWeaponActive("SMG") &&
               PlayerUpgradeManager.Instance.SelectedBoatName == "Battle Ship";
    }
}
