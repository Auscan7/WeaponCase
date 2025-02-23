using UnityEngine;
using static Upgrade;

[CreateAssetMenu(fileName = "ShotgunPelletCountUpgrade", menuName = "Upgrades/WeaponImprovement/Shotgun/ShotgunPelletCount")]
public class ShotgunPelletCountUpgrade : Upgrade, IConditionalUpgrade
{
    public int pelletCountIncrease;

    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        upgradeManager.baseShotgunProjectileCount = upgradeManager.baseShotgunProjectileCount + pelletCountIncrease;
    }

    public bool CanOffer()
    {
        return PlayerUpgradeManager.Instance.IsWeaponActive("Shotgun");
    }
}
