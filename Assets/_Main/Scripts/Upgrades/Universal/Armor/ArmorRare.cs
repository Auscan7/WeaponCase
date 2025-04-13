using UnityEngine;

[CreateAssetMenu(fileName = "ArmorRare", menuName = "Upgrades/Universal/Armor/ArmorRare")]
public class ArmorRare : Upgrade
{
    public float armorIncreaseAmount;
    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        // Increase the player's armor
        upgradeManager.playerArmor += armorIncreaseAmount;
    }
}
