using UnityEngine;

[CreateAssetMenu(fileName = "HPEpic", menuName = "Upgrades/Universal/HP/HPEpic")]
public class HPEpic : Upgrade
{
    public int hpIncreaseAmount;
    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        upgradeManager.playerMaxHealth += hpIncreaseAmount;
    }
}
