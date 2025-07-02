using UnityEngine;
using static Upgrade;

[CreateAssetMenu(fileName = "OrbitalStrikeActivateUpgrade", menuName = "Upgrades/WeaponUnlocks/OrbitalStrike")]
public class ActivateOrbitalStrikeUpgrade : Upgrade, IConditionalUpgrade
{
    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        upgradeManager.ActivateWeapon("OrbitalStrike");
    }
    public bool CanOffer()
    {
        return !PlayerUpgradeManager.Instance.IsWeaponActive("OrbitalStrike") &&
               PlayerUpgradeManager.Instance.SelectedBoatName == "Battle Ship" &&
           PlayerUpgradeManager.Instance.HasFreeWeaponSlot();
    }
}