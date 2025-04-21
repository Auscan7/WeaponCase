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
        // Only offer if the player selected the battle ship boat.
        // Assuming your UpgradeManager stores a weaponDict.
        return !PlayerUpgradeManager.Instance.IsWeaponActive("FlameCircle") && 
            PlayerUpgradeManager.Instance.SelectedBoatName == "Battle Ship";
    }
}

