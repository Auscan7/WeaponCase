using UnityEngine;

[CreateAssetMenu(fileName = "ArmorLegendary", menuName = "Upgrades/Universal/Armor/ArmorLegendary")]
public class ArmorLegendary : Upgrade
{
    public float armorIncreaseAmount;
    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        upgradeManager.playerArmor += armorIncreaseAmount;
    }
}
