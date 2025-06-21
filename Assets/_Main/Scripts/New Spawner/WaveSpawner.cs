using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public List<EnemyWaveData> waveDatas;  // List of enemy wave data
    private Dictionary<string, Transform> spawnZones;

    private void Awake()
    {
        // Cache all spawn zones in the scene
        spawnZones = new Dictionary<string, Transform>();
        foreach (var zone in GameObject.FindGameObjectsWithTag("SpawnZone"))
        {
            spawnZones[zone.name] = zone.transform;
        }
    }

    private IEnumerator Start()
    {
        // Wait until loadout selection is finished
        while (LoadoutSelectionManager.Instance != null && LoadoutSelectionManager.Instance.IsLoadoutActive)
        {
            yield return null;
        }

        // Start all wave spawns in parallel
        foreach (var waveData in waveDatas)
        {
            StartCoroutine(HandleWaveSpawns(waveData));
        }
    }

    private IEnumerator HandleWaveSpawns(EnemyWaveData waveData)
    {
        float waveStartTime = Time.time;

        List<SpawnEvent> pendingSpawns = new List<SpawnEvent>(waveData.spawnEvents);

        while (pendingSpawns.Count > 0)
        {
            float elapsed = Time.time - waveStartTime;

            for (int i = pendingSpawns.Count - 1; i >= 0; i--)
            {
                var spawn = pendingSpawns[i];

                if (elapsed >= spawn.spawnTime)
                {
                    if (spawnZones.TryGetValue(spawn.spawnZoneName, out Transform spawnZone))
                    {
                        StartCoroutine(SpawnEnemiesOverTime(spawn, spawnZone));
                    }
                    else
                    {
                        Debug.LogWarning($"Spawn zone '{spawn.spawnZoneName}' not found!");
                    }

                    pendingSpawns.RemoveAt(i);
                }
            }

            yield return null; // Wait for next frame
        }
    }

    private IEnumerator SpawnEnemiesOverTime(SpawnEvent spawn, Transform spawnZone)
    {
        for (int i = 0; i < spawn.amount; i++)
        {
            bool ignoreLimit = false;
            EnemyStatManager statManager = spawn.enemyPrefab.GetComponent<EnemyStatManager>();
            if (statManager != null && statManager.ignoreSpawnLimit)
            {
                ignoreLimit = true;
            }

            if (!EnemyLimitManager.Instance.CanSpawnEnemy(ignoreLimit))
            {
                yield return new WaitForSeconds(spawn.spawnInterval); // Delay then retry
                i--; // Try again in next loop iteration
                continue;
            }

            GameObject enemy = EnemyPoolManager.Instance.GetEnemy(spawn.enemyPrefab);
            if (enemy != null)
            {
                Vector3 spawnPosition = spawnZone.position;
                bool foundFreeSpot = false;
                float searchRadius = 2f;
                float checkRadius = 0.3f;
                int maxAttempts = 15;

                for (int attempt = 0; attempt < maxAttempts; attempt++)
                {
                    Vector2 randomOffset = Random.insideUnitCircle * searchRadius;
                    Vector3 candidatePosition = spawnZone.position + new Vector3(randomOffset.x, randomOffset.y, 0f);

                    if (!Physics2D.OverlapCircle(candidatePosition, checkRadius))
                    {
                        spawnPosition = candidatePosition;
                        foundFreeSpot = true;
                        break;
                    }
                }

                if (!foundFreeSpot)
                {
                    Debug.LogWarning($"[{spawn.enemyPrefab.name}] could not find non-overlapping spawn spot after {maxAttempts} tries. Using default zone position.");
                }

                enemy.transform.position = spawnPosition;
                enemy.transform.rotation = Quaternion.identity;
                EnemyLimitManager.Instance.RegisterEnemy();
            }

            if (i < spawn.amount - 1)
            {
                yield return new WaitForSeconds(spawn.spawnInterval);
            }
        }
    }
}
