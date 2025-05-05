using UnityEngine;

[CreateAssetMenu(fileName = "DodgeRare", menuName = "Upgrades/Universal/Dodge/DodgeRare")]
public class DodgeRare : Upgrade
{
    public int dodgeChanceIncrease;
    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        // Increase the player's dodge chance
        upgradeManager.playerDodgeChancePercent += dodgeChanceIncrease;
    }
}
