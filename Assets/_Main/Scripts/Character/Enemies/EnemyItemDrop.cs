using UnityEngine;

public class EnemyItemDrop : MonoBehaviour
{
    [System.Serializable]
    private struct ItemDropData
    {
        public GameObject prefab;
        public float dropChance;
    }

    [Header("Drop Items & Chances")]
    [SerializeField] private ItemDropData[] dropItems;

    private Vector2 positionOffset = new Vector2(0.5f, 0.5f);
    public Transform dropPoint;

    public void DropItem()
    {
        foreach (var item in dropItems)
        {
            TryDropItem(item);
        }
    }

    private void TryDropItem(ItemDropData item)
    {
        if (Random.Range(0f, 100f) <= item.dropChance)
        {
            Vector3 spawnPosition = dropPoint != null ? dropPoint.position : transform.position;
            spawnPosition.x += Random.Range(-positionOffset.x, positionOffset.x);
            spawnPosition.y += Random.Range(-positionOffset.y, positionOffset.y);

            Instantiate(item.prefab, spawnPosition, Quaternion.identity);
        }
    }
}