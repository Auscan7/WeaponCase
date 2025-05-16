using UnityEngine;
using static Upgrade;

[CreateAssetMenu(fileName = "ArrowHopCountUpgrade", menuName = "Upgrades/WeaponImprovement/BowAndArrow/ArrowHopCount")]
public class ArrowHopCount : Upgrade, IConditionalUpgrade
{
    public int hopCountIncrease;

    public override void ApplyUpgrade(PlayerUpgradeManager upgradeManager)
    {
        upgradeManager.baseBowAndArrowHopCount = upgradeManager.baseBowAndArrowHopCount + hopCountIncrease;
    }

    public bool CanOffer()
    {
        return PlayerUpgradeManager.Instance.IsWeaponActive("BowAndArrow");
    }
}
