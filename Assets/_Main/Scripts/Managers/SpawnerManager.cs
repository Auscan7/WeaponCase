using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public static SpawnerManager Instance { get; private set; }

    private List<EnemySpawner> spawners = new List<EnemySpawner>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void RegisterSpawner(EnemySpawner spawner)
    {
        if (!spawners.Contains(spawner))
            spawners.Add(spawner);
    }

    public void PauseAllSpawners()
    {
        Debug.Log("Pausing all spawners: " + spawners.Count);
        foreach (var spawner in spawners)
        {
            Debug.Log("Pausing spawner: " + spawner.name);
            spawner.PauseSpawning();
        }
    }

    public void ResumeAllSpawners()
    {
        foreach (var spawner in spawners)
        {
            spawner.ResumeSpawning();
        }
    }
}
