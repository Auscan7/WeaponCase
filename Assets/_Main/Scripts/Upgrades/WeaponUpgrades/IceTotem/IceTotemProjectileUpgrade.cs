using UnityEngine;
using static Upgrade;

[CreateAssetMenu(fileName = "IceTotemProjectileUpgrade", menuName = "Upgrades/WeaponImprovement/IceTotem/Projectile Upgrade")]
public class IceTotemProjectileUpgrade : Upgrade, IConditionalUpgrade
{
    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        upgradeManager.baseIceTotemProjectileCount++;
    }
    public bool CanOffer()
    {
        // Only offer if the Ice Totem Projectile is not active yet.
        return PlayerUpgradeManager.Instance.IsWeaponActive("IceTotem");
    }
}