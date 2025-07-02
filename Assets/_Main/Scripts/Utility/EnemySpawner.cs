using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject[] enemyPrefabs;
    public float spawnInterval = 5f;
    public int maxSpawnAmount = 10;
    public float initialSpawnDelay = 3f;

    private int spawnedCount = 0;
    private bool isPaused = false;

    private void Start()
    {
        SpawnerManager.Instance.RegisterSpawner(this);
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        float delayTimer = 0f;

        // Manual initial delay that respects pause
        while (delayTimer < initialSpawnDelay)
        {
            if (!isPaused)
                delayTimer += Time.deltaTime;

            yield return null;
        }

        while (spawnedCount < maxSpawnAmount)
        {
            while (isPaused)
                yield return null;

            SpawnEnemy();
            spawnedCount++;

            float timer = 0f;
            while (timer < spawnInterval)
            {
                if (!isPaused)
                    timer += Time.deltaTime;

                yield return null;
            }
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefabs.Length == 0)
        {
            Debug.LogWarning("No enemy prefabs assigned!");
            return;
        }

        GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        GameObject enemy = EnemyPoolManager.Instance.GetEnemy(enemyPrefab);
        if (enemy != null)
        {
            enemy.transform.position = transform.position;
            enemy.transform.rotation = Quaternion.identity;
        }
    }

    // Call these methods to control the pause state
    public void PauseSpawning()
    {
        Debug.Log(name + " paused");
        isPaused = true;
    }

    public void ResumeSpawning()
    {
        Debug.Log(name + " resumed");
        isPaused = false;
    }
}
