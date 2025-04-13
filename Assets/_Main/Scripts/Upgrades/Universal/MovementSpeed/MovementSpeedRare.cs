using UnityEngine;

[CreateAssetMenu(fileName = "MovementSpeedRare", menuName = "Upgrades/Universal/MovementSpeed/MovementSpeedRare")]
public class MovementSpeedRare : Upgrade
{
    public float movementSpeedIncrease;
    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        // Increase the player's movement speed
        upgradeManager.playerMovementSpeedMultiplier += upgradeManager.playerMovementSpeedMultiplier / 10f * movementSpeedIncrease;
    }
}
