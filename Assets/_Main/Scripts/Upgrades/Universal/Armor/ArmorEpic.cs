using UnityEngine;

[CreateAssetMenu(fileName = "ArmorEpic", menuName = "Upgrades/Universal/Armor/ArmorEpic")]
public class ArmorEpic : Upgrade
{
    public float armorIncreaseAmount;
    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        upgradeManager.playerArmor += armorIncreaseAmount;
    }
}
