using UnityEngine;
using static Upgrade;

[CreateAssetMenu(fileName = "ActivateIceTotemUpgrade", menuName = "Upgrades/WeaponUnlocks/IceTotem")]
public class ActivateIceTotemUpgrade : Upgrade, IConditionalUpgrade
{
    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        upgradeManager.ActivateWeapon("IceTotem");
    }
    public bool CanOffer()
    {
        // Only offer if the Ice Totem is not active yet.
        // Assuming your UpgradeManager stores a weaponDict.
        return !PlayerUpgradeManager.Instance.IsWeaponActive("IceTotem") &&
           PlayerUpgradeManager.Instance.HasFreeWeaponSlot();
    }
}
