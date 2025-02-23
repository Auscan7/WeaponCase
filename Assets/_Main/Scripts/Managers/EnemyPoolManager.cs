using UnityEngine;
using System.Collections.Generic;

public class EnemyPoolManager : MonoBehaviour
{
    public static EnemyPoolManager Instance { get; private set; }

    [System.Serializable]
    public class EnemyPoolItem
    {
        public GameObject enemyPrefab;
        public int poolSize = 20;
    }

    [SerializeField] private EnemyPoolItem[] enemyPoolItems;

    // Dictionary to hold pools for each enemy type
    private Dictionary<GameObject, Queue<GameObject>> enemyPools = new Dictionary<GameObject, Queue<GameObject>>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializePools();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializePools()
    {
        foreach (var item in enemyPoolItems)
        {
            Queue<GameObject> poolQueue = new Queue<GameObject>();
            for (int i = 0; i < item.poolSize; i++)
            {
                GameObject enemy = Instantiate(item.enemyPrefab);
                enemy.SetActive(false);
                poolQueue.Enqueue(enemy);
            }
            enemyPools[item.enemyPrefab] = poolQueue;
        }
    }

    // Gets an enemy from the pool based on the prefab
    public GameObject GetEnemy(GameObject enemyPrefab)
    {
        if (enemyPools.ContainsKey(enemyPrefab))
        {
            Queue<GameObject> pool = enemyPools[enemyPrefab];
            GameObject enemy;
            if (pool.Count > 0)
            {
                enemy = pool.Dequeue();
                enemy.SetActive(true);
            }
            else
            {
                Debug.LogWarning("Enemy pool for prefab is empty! Instantiating new enemy.");
                enemy = Instantiate(enemyPrefab);
            }

            // Force the enemy to remember its original prefab asset.
            EnemyStatManager enemyScript = enemy.GetComponent<EnemyStatManager>(); // If you have a base enemy script, or use CharacterStatManager if that's on the enemy.
            if (enemyScript != null)
            {
                enemyScript.enemyPrefabReference = enemyPrefab;
            }
            return enemy;
        }
        else
        {
            Debug.LogError("No pool found for the requested enemy prefab.");
            return null;
        }
    }


    // Returns the enemy to the pool
    public void ReturnEnemy(GameObject enemy, GameObject enemyPrefab)
    {
        enemy.SetActive(false);
        if (enemyPools.ContainsKey(enemyPrefab))
        {
            enemyPools[enemyPrefab].Enqueue(enemy);
            enemy.GetComponent<CharacterStatManager>().ResetEnemy();
        }
        else
        {
            Debug.LogWarning("No pool found for enemy prefab when returning enemy.");
        }
    }
}
