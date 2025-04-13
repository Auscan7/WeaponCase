using UnityEngine;

[CreateAssetMenu(fileName = "ArmorUnique", menuName = "Upgrades/Universal/Armor/ArmorUnique")]
public class ArmorUnique : Upgrade
{
    public float armorIncreaseAmount;
    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        upgradeManager.playerArmor += armorIncreaseAmount;
    }
}
