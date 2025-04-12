using UnityEngine;

[CreateAssetMenu(fileName = "MovementSpeedUnique", menuName = "Upgrades/MovementSpeed/MovementSpeedUnique")]
public class MovementSpeedUnique : Upgrade
{
    public float movementSpeedIncrease;
    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        // Increase the player's movement speed
        upgradeManager.playerMovementSpeedMultiplier += upgradeManager.playerMovementSpeedMultiplier / 10f * movementSpeedIncrease;
    }
}
