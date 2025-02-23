using UnityEngine;

public abstract class Upgrade : ScriptableObject
{
    public string upgradeName;
    public string positiveDescription;
    public string negativeDescription;
    public Sprite icon;
    public Sprite border;

    // Each upgrade will define its own logic
    public abstract void ApplyUpgrade(PlayerUpgradeManager upgradeManager);

    public interface IConditionalUpgrade
    {
        /// <summary>
        /// Returns true if the upgrade should be offered.
        /// </summary>
        bool CanOffer();
    }
}
