using UnityEngine;

[CreateAssetMenu(fileName = "MovementSpeedCommon", menuName = "Upgrades/Universal/MovementSpeed/MovementSpeedCommon")]
public class MovementSpeedCommon : Upgrade
{
    public float movementSpeedIncrease;
    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        // Increase the player's movement speed
        upgradeManager.playerMovementSpeedMultiplier += upgradeManager.playerMovementSpeedMultiplier / 10f * movementSpeedIncrease;
    }
}
