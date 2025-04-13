using UnityEngine;

[CreateAssetMenu(fileName = "HPCommon", menuName = "Upgrades/Universal/HP/HPCommon")]
public class HPCommon : Upgrade
{
    public int hpIncreaseAmount;
    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        upgradeManager.playerMaxHealth += hpIncreaseAmount;
        upgradeManager.playerCurrentHealth += hpIncreaseAmount;
    }
}
