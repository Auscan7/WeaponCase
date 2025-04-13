using UnityEngine;

[CreateAssetMenu(fileName = "MagnetCommon", menuName = "Upgrades/Universal/Magnet/MagnetCommon")]
public class MagnetCommon : Upgrade
{
    public float magnetRadiusIncrease;

    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        // Increase the player's magnet radius
        upgradeManager.playerMagnetRadius += upgradeManager.playerMagnetRadius / 10f * magnetRadiusIncrease;
    }
}
