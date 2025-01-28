using UnityEngine;

public class GemDrop : MonoBehaviour
{
    public GameObject gemPrefab;
    public Transform dropPoint;

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