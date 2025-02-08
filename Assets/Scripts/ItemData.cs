using UnityEngine;

[CreateAssetMenu(menuName = "item")]
public class ItemData : ScriptableObject
{
    public int width = 1;
    public int height = 1;

    public Sprite itemIcon;
}
