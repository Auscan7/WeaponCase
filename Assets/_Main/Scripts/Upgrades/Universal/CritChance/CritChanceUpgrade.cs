using UnityEngine;

[CreateAssetMenu(fileName = "CritChanceUpgrade", menuName = "Upgrades/Universal/CritChance/CritChanceUpgrade")]
public class CritChanceUpgrade : Upgrade
{
    public float critChanceIncrease;

    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        upgradeManager.playerCritChancePercent += critChanceIncrease;
    }
}
