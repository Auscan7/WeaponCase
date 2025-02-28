using UnityEngine;
using static Upgrade;

[CreateAssetMenu(fileName = "GrenadeActivateUpgrade", menuName = "Upgrades/WeaponUnlocks/Grenade")]
public class ActivateGrenadeUpgrade : Upgrade, IConditionalUpgrade
{
    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        upgradeManager.ActivateWeapon("Grenade");
    }
    public bool CanOffer()
    {
        return !PlayerUpgradeManager.Instance.IsWeaponActive("Grenade");
    }
}
