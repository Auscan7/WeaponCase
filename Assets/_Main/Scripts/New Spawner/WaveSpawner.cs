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
            GameObject enemy = EnemyPoolManager.Instance.GetEnemy(spawn.enemyPrefab);
            if (enemy != null)
            {
                enemy.transform.position = spawnZone.position;
                enemy.transform.rotation = Quaternion.identity;
            }

            if (i < spawn.amount - 1) // Wait between spawns, skip after last
            {
                yield return new WaitForSeconds(spawn.spawnInterval);
            }
        }
    }
}
