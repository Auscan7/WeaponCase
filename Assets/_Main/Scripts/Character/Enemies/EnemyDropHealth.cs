using UnityEngine;

public class EnemyDropHealth : MonoBehaviour
{
    [SerializeField] private GameObject dropItemPrefab;
    [SerializeField] private float dropChance = 50f;
    [SerializeField] private Vector2 positionOffset = new Vector2(0.5f, 0.5f);
    public Transform dropPoint;

    public void DropHealthItem()
    {
        float randomValue = Random.Range(0f, 100f);

        if (randomValue <= dropChance)
        {
            Vector3 spawnPosition = dropPoint != null ? dropPoint.position : transform.position;
            spawnPosition.x += Random.Range(-positionOffset.x, positionOffset.x);
            spawnPosition.y += Random.Range(-positionOffset.y, positionOffset.y);

            // Instantiate the drop item
            Instantiate(dropItemPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
