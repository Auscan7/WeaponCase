using UnityEngine;
using static Upgrade;

[CreateAssetMenu(fileName = "FlameCircleActivateUpgrade", menuName = "Upgrades/WeaponUnlocks/FlameCircle")]
public class ActivateFlameCircleUpgrade : Upgrade, IConditionalUpgrade
{
    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        upgradeManager.ActivateWeapon("FlameCircle");
    }
    public bool CanOffer()
    {
        // Only offer if the Flame Circle is not active yet.
        // Assuming your UpgradeManager stores a weaponDict.
        return !PlayerUpgradeManager.Instance.IsWeaponActive("FlameCircle") &&
           PlayerUpgradeManager.Instance.HasFreeWeaponSlot();
    }
}

