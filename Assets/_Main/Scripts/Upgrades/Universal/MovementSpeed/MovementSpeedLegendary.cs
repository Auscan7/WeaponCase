using UnityEngine;

[CreateAssetMenu(fileName = "MovementSpeedLegendary", menuName = "Upgrades/MovementSpeed/MovementSpeedLegendary")]
public class MovementSpeedLegendary : Upgrade
{
    public float movementSpeedIncrease;
    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        // Increase the player's movement speed
        upgradeManager.playerMovementSpeedMultiplier += upgradeManager.playerMovementSpeedMultiplier / 10f * movementSpeedIncrease;
    }
}
