using UnityEngine;

[CreateAssetMenu(fileName = "DodgeEpic", menuName = "Upgrades/Universal/Dodge/DodgeEpic")]
public class DodgeEpic : Upgrade
{
    public int dodgeChanceIncrease;
    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        // Increase the player's dodge chance
        upgradeManager.playerDodgeChancePercent += dodgeChanceIncrease;
    }
}
