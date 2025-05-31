using UnityEngine;

[CreateAssetMenu(fileName = "HPRegenUpgrade", menuName = "Upgrades/Universal/HPRegenUpgrade")]
public class HPRegenUpgrade : Upgrade
{
    public float increseAmount;

    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        upgradeManager.playerHealthRegenAmount += increseAmount;
    }
}
