using UnityEngine;

[CreateAssetMenu(fileName = "CritDamageUpgrade", menuName = "Upgrades/Universal/CritDamage/CritDamageUpgrade")]
public class CritDamageUpgrade : Upgrade
{
    public float critDamageIncrease;
    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        upgradeManager.playerCritDamageMultiplier += critDamageIncrease;
    }
}
