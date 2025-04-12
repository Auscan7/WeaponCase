using UnityEngine;

[CreateAssetMenu(fileName = "MagnetUnique", menuName = "Upgrades/Magnet/MagnetUnique")]
public class MagnetUnique : Upgrade
{
    public float magnetRadiusIncrease;
    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        // Increase the player's magnet radius
        upgradeManager.playerMagnetRadius += upgradeManager.playerMagnetRadius / 10f * magnetRadiusIncrease;
    }
}
