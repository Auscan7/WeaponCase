using UnityEngine;

public class VortexSpawner : MonoBehaviour
{
    [SerializeField] private GameObject vortexPrefab; // Prefab of the vortex
    [SerializeField] private Transform player; // Reference to the player's transform
    [SerializeField] private float spawnDistance = 5f; // Distance from the player to spawn the vortex
    [SerializeField] private float vortexSpeed = 5f; // Speed of the vortex's movement
    [SerializeField] private float minSpawnTime = 30f; // Minimum time before a vortex spawns
    [SerializeField] private float maxSpawnTime = 60f; // Maximum time before a vortex spawns

    private void Start()
    {
        // Schedule the first vortex spawn
        ScheduleNextVortex();
    }

    private void ScheduleNextVortex()
    {
        float randomDelay = Random.Range(minSpawnTime, maxSpawnTime); // Get a random time between min and max
        Invoke(nameof(SpawnVortex), randomDelay); // Schedule the vortex spawn
    }

    public void SpawnVortex()
    {
        if (player == null)
        {
            Debug.LogWarning("Player reference not set!");
            return;
        }

        // Calculate a random spawn position around the player
        Vector2 randomDirection = Random.insideUnitCircle.normalized; // Random direction
        Vector3 spawnPosition = player.position + (Vector3)(randomDirection * spawnDistance);

        // Instantiate the vortex
        GameObject vortex = Instantiate(vortexPrefab, spawnPosition, vortexPrefab.transform.rotation);

        // Get the direction to the player's position
        Vector3 routeDirection = (player.position - spawnPosition).normalized;

        // Assign movement logic to the vortex
        VortexMovement vortexMovement = vortex.GetComponent<VortexMovement>();
        if (vortexMovement != null)
        {
            vortexMovement.Initialize(routeDirection, vortexSpeed);
        }
        else
        {
            Debug.LogError("Vortex prefab is missing the VortexMovement component.");
        }

        // Reset the time frame for the next vortex spawn
        ScheduleNextVortex();
    }
}
