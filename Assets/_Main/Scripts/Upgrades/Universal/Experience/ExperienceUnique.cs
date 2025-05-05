using UnityEngine;

[CreateAssetMenu(fileName = "ExperienceUnique", menuName = "Upgrades/Universal/Experience/ExperienceUnique")]
public class ExperienceUnique : Upgrade
{
    public float experienceMultiplierIncrease;
    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        upgradeManager.playerXpMultiplier += upgradeManager.playerXpMultiplier / 10f * experienceMultiplierIncrease;
    }
}
