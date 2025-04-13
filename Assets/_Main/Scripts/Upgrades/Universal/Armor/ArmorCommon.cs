using UnityEngine;

[CreateAssetMenu(fileName = "ArmorCommon", menuName = "Upgrades/Universal/Armor/ArmorCommon")]
public class ArmorCommon : Upgrade
{
    public float armorIncreaseAmount;

    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        upgradeManager.playerArmor += armorIncreaseAmount;
    }
}
