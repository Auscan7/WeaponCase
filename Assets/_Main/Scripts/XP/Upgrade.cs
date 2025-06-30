using UnityEngine;

public abstract class Upgrade : ScriptableObject
{
    public string upgradeName;
    public string positiveDescription;
    public string negativeDescription;
    public Sprite icon;
    public Sprite border;

    public enum UpgradeType { Stat, Weapon, Other }
    public UpgradeType upgradeType;

    public abstract void ApplyUpgrade(PlayerUpgradeManager upgradeManager);

    public interface IConditionalUpgrade
    {
        bool CanOffer();
    }
}
