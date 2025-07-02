using UnityEngine;
using static Upgrade;

[CreateAssetMenu(fileName = "PoisonTotemDuration", menuName = "Upgrades/WeaponImprovement/PoisonTotem/PoisonTotemDuration")]
public class PoisonTotemDuration : Upgrade, IConditionalUpgrade
{
    public float durationIncrease;
    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        upgradeManager.poisonTotemStats.duration += durationIncrease;
    }
    public bool CanOffer()
    {
        // Only offer if the Poison Totem is active.
        return PlayerUpgradeManager.Instance.IsWeaponActive("PoisonTotem");
    }
}
