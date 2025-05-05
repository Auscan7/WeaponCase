using UnityEngine;

[CreateAssetMenu(fileName = "ExperienceRare", menuName = "Upgrades/Universal/Experience/ExperienceRare")]
public class ExperienceRare : Upgrade
{
    public float experienceMultiplierIncrease;
    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        upgradeManager.playerXpMultiplier += upgradeManager.playerXpMultiplier / 10f * experienceMultiplierIncrease;
    }
}
