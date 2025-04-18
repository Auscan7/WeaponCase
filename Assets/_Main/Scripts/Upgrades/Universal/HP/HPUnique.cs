using UnityEngine;

[CreateAssetMenu(fileName = "HPUnique", menuName = "Upgrades/Universal/HP/HPUnique")]
public class HPUnique : Upgrade
{
    public int hpIncreaseAmount;
    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        upgradeManager.playerMaxHealth += hpIncreaseAmount;
    }
}