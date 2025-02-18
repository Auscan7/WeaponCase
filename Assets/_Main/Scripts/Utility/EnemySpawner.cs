using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject[] enemyPrefabs;      // Array of enemy prefabs
    public float spawnInterval = 5f;       // Time between spawns
    public int maxSpawnAmount = 10;        // Maximum enemies to spawn
    public float initialSpawnDelay = 3f;   // Delay before the first spawn

    private int spawnedCount = 0;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(initialSpawnDelay);

        while (spawnedCount < maxSpawnAmount)
        {
            SpawnEnemy();
            spawnedCount++;
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefabs.Length == 0)
        {
            Debug.LogWarning("No enemy prefabs assigned!");
            return;
        }

        // Choose a random enemy prefab
        GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        // Get the enemy from the pool
        GameObject enemy = EnemyPoolManager.Instance.GetEnemy(enemyPrefab);
        if (enemy != null)
        {
            enemy.transform.position = transform.position;
            enemy.transform.rotation = Quaternion.identity;
        }
    }
}
