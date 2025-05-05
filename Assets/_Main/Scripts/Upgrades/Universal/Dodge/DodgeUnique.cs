using UnityEngine;

[CreateAssetMenu(fileName = "DodgeUnique", menuName = "Upgrades/Universal/Dodge/DodgeUnique")]
public class DodgeUnique : Upgrade
{
    public int dodgeChanceIncrease;
    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        // Increase the player's dodge chance
        upgradeManager.playerDodgeChancePercent += dodgeChanceIncrease;
    }
}
