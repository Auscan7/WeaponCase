using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgrade", menuName = "Upgrades/Upgrade")]
public class Upgrade : ScriptableObject
{
    public string upgradeName;
    public string positiveDescription;
    public string negativeDescription;
    public Sprite icon;
    public Sprite border;
    public int UpgradeID;
}
