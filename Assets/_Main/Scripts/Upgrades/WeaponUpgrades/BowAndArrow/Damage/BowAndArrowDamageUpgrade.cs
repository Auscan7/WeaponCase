using UnityEngine;
using static Upgrade;

[CreateAssetMenu(fileName = "BowAndArrowDamageUpgrade", menuName = "Upgrades/WeaponImprovement/BowAndArrow/BowAndArrowDamage")]
public class BowAndArrowDamageUpgrade : Upgrade, IConditionalUpgrade
{
    public float damageIncrease;

    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        upgradeManager.bowAndArrowDamageMultiplier += damageIncrease / 10f;
        upgradeManager.UpdateWeaponDamage();
    }

    public bool CanOffer()
    {
        // Only offer if the bow and arrow is active.
        return PlayerUpgradeManager.Instance.IsWeaponActive("BowAndArrow");
    }
}
