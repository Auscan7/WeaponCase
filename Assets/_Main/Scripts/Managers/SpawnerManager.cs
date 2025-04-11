using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public static SpawnerManager Instance { get; private set; }

    private List<EnemySpawner> spawners = new List<EnemySpawner>();

    [SerializeField]private VortexSpawner vortexSpawner;

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
        foreach (var spawner in spawners)
            spawner.PauseSpawning();

        vortexSpawner.PauseSpawning();
    }

    public void ResumeAllSpawners()
    {
        foreach (var spawner in spawners)
            spawner.ResumeSpawning();

        vortexSpawner.ResumeSpawning();
    }
}
