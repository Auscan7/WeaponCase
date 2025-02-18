using UnityEngine;
using System.Collections.Generic;

public class GemPoolManager : MonoBehaviour
{
    public static GemPoolManager Instance { get; private set; }

    [SerializeField] private List<GemPoolItem> gemPoolItems;

    // Dictionary mapping gem types to their pool queues
    private Dictionary<GemType, Queue<GameObject>> gemPools = new Dictionary<GemType, Queue<GameObject>>();

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
        foreach (GemPoolItem item in gemPoolItems)
        {
            Queue<GameObject> poolQueue = new Queue<GameObject>();
            for (int i = 0; i < item.poolSize; i++)
            {
                GameObject gem = Instantiate(item.gemPrefab);
                gem.SetActive(false);
                poolQueue.Enqueue(gem);
            }
            gemPools[item.gemType] = poolQueue;
        }
    }

    public GameObject GetGem(GemType type, Vector3 spawnPosition)
    {
        if (gemPools.TryGetValue(type, out Queue<GameObject> pool))
        {
            if (pool.Count > 0)
            {
                GameObject gem = pool.Dequeue();
                gem.SetActive(true);
                gem.transform.position = spawnPosition; // Set the position here
                return gem;
            }
            else
            {
                Debug.LogWarning($"Gem pool for {type} is empty! Instantiating a new gem.");
                // Find the corresponding GemPoolItem to instantiate a new gem.
                GemPoolItem poolItem = gemPoolItems.Find(item => item.gemType == type);
                if (poolItem != null)
                {
                    GameObject newGem = Instantiate(poolItem.gemPrefab);
                    newGem.transform.position = spawnPosition; // Set the position here
                    return newGem;
                }
            }
        }
        Debug.LogError("Invalid gem type requested or no pool found.");
        return null;
    }

    public void ReturnGem(GemType type, GameObject gem)
    {
        gem.SetActive(false);
        if (gemPools.ContainsKey(type))
        {
            gemPools[type].Enqueue(gem);
        }
        else
        {
            Debug.LogWarning("No pool exists for gem type: " + type);
        }
    }

    public enum GemType
    {
        Ruby,
        Sapphire,
        Emerald,
        Gold,
        Diamond
        // Add other gem types as needed
    }


    [System.Serializable]
    public class GemPoolItem
    {
        public GemType gemType;
        public GameObject gemPrefab;
        public int poolSize = 20; // Set default pool size here
    }

}