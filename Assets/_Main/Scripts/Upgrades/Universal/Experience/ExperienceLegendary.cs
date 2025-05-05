using UnityEngine;

[CreateAssetMenu(fileName = "ExperienceLegendary", menuName = "Upgrades/Universal/Experience/ExperienceLegendary")]
public class ExperienceLegendary : Upgrade
{
    public float experienceMultiplierIncrease;
    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        upgradeManager.playerXpMultiplier += upgradeManager.playerXpMultiplier / 10f * experienceMultiplierIncrease;
    }
}
