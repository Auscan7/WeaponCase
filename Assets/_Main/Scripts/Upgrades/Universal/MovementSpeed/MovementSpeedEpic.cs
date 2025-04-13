using UnityEngine;

[CreateAssetMenu(fileName = "MovementSpeedEpic", menuName = "Upgrades/Universal/MovementSpeed/MovementSpeedEpic")]
public class MovementSpeedEpic : Upgrade
{
    public float movementSpeedIncrease;
    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        // Increase the player's movement speed
        upgradeManager.playerMovementSpeedMultiplier += upgradeManager.playerMovementSpeedMultiplier / 10f * movementSpeedIncrease;
    }
}
