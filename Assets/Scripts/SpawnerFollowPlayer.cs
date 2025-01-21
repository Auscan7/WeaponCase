using UnityEngine;

public class SpawnerFollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform player; // Reference to the player's transform

    void Update()
    {
        if (player != null)
        {
            Vector3 targetPosition = player.position;
            transform.position = player.position;
        }
    }
}
