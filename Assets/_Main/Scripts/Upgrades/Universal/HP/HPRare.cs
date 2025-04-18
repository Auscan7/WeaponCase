using UnityEngine;

[CreateAssetMenu(fileName = "HPRare", menuName = "Upgrades/Universal/HP/HPRare")]
public class HPRare : Upgrade
{
    public int hpIncreaseAmount;
    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        upgradeManager.playerMaxHealth += hpIncreaseAmount;
    }
}
