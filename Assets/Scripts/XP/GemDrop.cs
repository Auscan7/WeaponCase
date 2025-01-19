using UnityEngine;

public class GemDrop : MonoBehaviour
{
    public GameObject gemPrefab; // Prefab for the gem to drop
    public Transform dropPoint; // Optional: A specific point to drop the gem

    public void DropGem()
    {
        if (gemPrefab == null)
        {
            Debug.LogError("Gem prefab not assigned.");
            return;
        }

        Vector3 spawnPosition = dropPoint != null ? dropPoint.position : transform.position;
        Instantiate(gemPrefab, spawnPosition, Quaternion.identity);
    }
}