using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject[] enemyPrefabs; // Reference for enemy prefabs
    public float spawnInterval = 5f; // Interval between spawns
    public int maxSpawnAmount = 10; // Max enemies to spawn
    public float initialSpawnDelay = 300f; // Delay before first spawn

    private int spawnedCount = 0;
    private float spawnTimer;

    void Start()
    {
        spawnTimer = initialSpawnDelay;
    }

    void Update()
    {
        if (spawnedCount >= maxSpawnAmount) return;

        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            SpawnEnemy();
            spawnTimer = spawnInterval; // Reset timer after initial delay
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefabs.Length == 0) return;

        GameObject enemyToSpawn = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        Instantiate(enemyToSpawn, transform.position, Quaternion.identity);
        spawnedCount++;
    }
}