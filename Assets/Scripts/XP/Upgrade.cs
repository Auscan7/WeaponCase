using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgrade", menuName = "Upgrades/Upgrade")]
public class Upgrade : ScriptableObject
{
    public string upgradeName;
    public string description;
    public Sprite icon;
    public Sprite border;
    public int UpgradeID;
}
