using UnityEngine;

[CreateAssetMenu(fileName = "ExperienceEpic", menuName = "Upgrades/Universal/Experience/ExperienceEpic")]
public class ExperienceEpic : Upgrade
{
    public float experienceMultiplierIncrease;
    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        upgradeManager.playerXpMultiplier += upgradeManager.playerXpMultiplier / 10f * experienceMultiplierIncrease;
    }
}
