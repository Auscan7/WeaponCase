using UnityEngine;
using static Upgrade;

[CreateAssetMenu(fileName = "ActivatePoisonTotemUpgrade", menuName = "Upgrades/WeaponUnlocks/PoisonTotem")]
public class ActivatePoisonTotem : Upgrade, IConditionalUpgrade
{
    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        upgradeManager.ActivateWeapon("PoisonTotem");
    }
    public bool CanOffer()
    {
        // Only offer if the Poison Totem is not active yet.
        return !PlayerUpgradeManager.Instance.IsWeaponActive("PoisonTotem") &&
           PlayerUpgradeManager.Instance.HasFreeWeaponSlot();
    }
}
