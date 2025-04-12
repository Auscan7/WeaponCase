using UnityEngine;

[CreateAssetMenu(fileName = "MagnetLegendary", menuName = "Upgrades/Magnet/MagnetLegendary")]
public class MagnetLegendary : Upgrade
{
    public float magnetRadiusIncrease;
    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        // Increase the player's magnet radius
        upgradeManager.playerMagnetRadius += upgradeManager.playerMagnetRadius / 10f * magnetRadiusIncrease;
    }
}
