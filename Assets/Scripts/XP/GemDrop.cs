using UnityEngine;
using static GemPoolManager;

public class GemDrop : MonoBehaviour
{
    // Instead of a gem prefab reference, we now specify the gem type to drop.
    public GemType gemType;
    public Transform dropPoint;

    public void DropGem()
    {
        Vector3 spawnPosition = dropPoint != null ? dropPoint.position : transform.position;

        // Get the gem from the pool manager based on gem type.
        GameObject droppedGem = GemPoolManager.Instance.GetGem(gemType, spawnPosition);
        if (droppedGem != null)
        {
            droppedGem.transform.rotation = Quaternion.identity;
        }
        else
        {
            Debug.LogError("Failed to retrieve gem from the pool.");
        }
    }
}
