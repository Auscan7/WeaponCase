using UnityEngine;

[CreateAssetMenu(fileName = "HPLegendary", menuName = "Upgrades/Universal/HP/HPLegendary")]
public class HPLegendary : Upgrade
{
    public int hpIncreaseAmount;
    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        upgradeManager.playerMaxHealth += hpIncreaseAmount;
    }
}
