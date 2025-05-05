using UnityEngine;

[CreateAssetMenu(fileName = "DodgeCommon", menuName = "Upgrades/Universal/Dodge/DodgeCommon")]
public class DodgeCommon : Upgrade
{
    public int dodgeChanceIncrease;
    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        // Increase the player's dodge chance
        upgradeManager.playerDodgeChancePercent += dodgeChanceIncrease;
    }
}