using UnityEngine;

[CreateAssetMenu(fileName = "ExperienceUpgrade", menuName = "Upgrades/Universal/ExperienceUpgrade")]
public class ExperienceUpgrade : Upgrade
{
    public float experienceMultiplierIncrease;
    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        upgradeManager.playerXpMultiplier += experienceMultiplierIncrease;
    }
}
