using UnityEngine;

[CreateAssetMenu(fileName = "MovementSpeedUpgrade", menuName = "Upgrades/Universal/MovementSpeedUpgrade")]
public class MovementSpeedUpgrade : Upgrade
{
    public float movementSpeedIncrease;
    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        // Increase the player's movement speed
        upgradeManager.playerMovementSpeedMultiplier += movementSpeedIncrease;
    }
}
