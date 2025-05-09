using UnityEngine;

[CreateAssetMenu(fileName = "MagnetRare", menuName = "Upgrades/Universal/Magnet/MagnetRare")]
public class MagnetRare : Upgrade
{
    public float magnetRadiusIncrease;

    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        // Increase the player's magnet radius
        upgradeManager.playerMagnetRadius += upgradeManager.playerMagnetRadius / 10f * magnetRadiusIncrease;
    }
}
