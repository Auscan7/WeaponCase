using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spawning/New Enemy Wave")]
public class EnemyWaveData : ScriptableObject
{
    public List<SpawnEvent> spawnEvents;
}

[Serializable]
public class SpawnEvent
{
    public GameObject enemyPrefab;
    public float spawnTime;
    public int amount = 1;
    public float spawnInterval = 0;

    [SpawnZoneSelector]
    public string spawnZoneName;
}
