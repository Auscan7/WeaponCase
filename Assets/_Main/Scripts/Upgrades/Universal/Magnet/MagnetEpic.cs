using UnityEngine;

[CreateAssetMenu(fileName = "MagnetEpic", menuName = "Upgrades/Magnet/MagnetEpic")]
public class MagnetEpic : Upgrade
{
    public float magnetRadiusIncrease;
    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        // Increase the player's magnet radius
        upgradeManager.playerMagnetRadius += upgradeManager.playerMagnetRadius / 10f * magnetRadiusIncrease;
    }
}