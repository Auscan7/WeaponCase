using UnityEngine;
using static Upgrade;

[CreateAssetMenu(fileName = "PistolPierceUpgrade", menuName = "Upgrades/WeaponImprovement/Pistol/Pierce")]
public class PistolPierceUpgrade : Upgrade, IConditionalUpgrade
{
    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        upgradeManager.basePistolPierceCount++;
    }
    public bool CanOffer()
    {
        return PlayerUpgradeManager.Instance.IsWeaponActive("Pistol");
    }
}
