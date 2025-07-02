using UnityEngine;
using static Upgrade;

[CreateAssetMenu(fileName = "BlowDartActivateUpgrade", menuName = "Upgrades/WeaponUnlocks/BlowDart")]
public class ActivateBlowDartUpgrade : Upgrade, IConditionalUpgrade
{

    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        upgradeManager.ActivateWeapon("BlowDart");
    }
    public bool CanOffer()
    {
        // Only offer if the blow dart is not active yet.
        // Only offer if the player selected the wooden boat.
        // Assuming your UpgradeManager stores a weaponDict.
        return !PlayerUpgradeManager.Instance.IsWeaponActive("BlowDart") &&
               PlayerUpgradeManager.Instance.SelectedBoatName == "Raft" &&
           PlayerUpgradeManager.Instance.HasFreeWeaponSlot();
    }
}
