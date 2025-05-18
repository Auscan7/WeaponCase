using UnityEngine;

[CreateAssetMenu(fileName = "MagnetUpgrade", menuName = "Upgrades/Universal/MagnetUpgrade")]
public class MagnetUpgrade : Upgrade
{
    public float magnetRadiusIncrease;

    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        // Increase the player's magnet radius
        upgradeManager.playerMagnetRadius += magnetRadiusIncrease;
    }
}
