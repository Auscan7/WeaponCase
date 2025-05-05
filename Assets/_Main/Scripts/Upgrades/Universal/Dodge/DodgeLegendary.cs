using UnityEngine;

[CreateAssetMenu(fileName = "DodgeLegendary", menuName = "Upgrades/Universal/Dodge/DodgeLegendary")]
public class DodgeLegendary : Upgrade
{
    public int dodgeChanceIncrease;
    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        // Increase the player's dodge chance
        upgradeManager.playerDodgeChancePercent += dodgeChanceIncrease;
    }
}
