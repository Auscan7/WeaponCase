using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgrade", menuName = "Upgrades/Upgrade")]
public class Upgrade : ScriptableObject
{
    public string upgradeName; // Name of the upgrade
    public string description; // Description of the upgrade
    public Sprite icon; // Icon for the upgrade
    public int UpgradeID;
}
