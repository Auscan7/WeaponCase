using UnityEngine;

[CreateAssetMenu(fileName = "ExperienceCommon", menuName = "Upgrades/Universal/Experience/ExperienceCommon")]
public class ExperienceCommon : Upgrade
{
    public float experienceMultiplierIncrease;
    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        upgradeManager.playerXpMultiplier += upgradeManager.playerXpMultiplier / 10f * experienceMultiplierIncrease;
    }
}
