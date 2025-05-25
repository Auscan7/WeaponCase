using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OctopusEnemyCombat : MonoBehaviour
{
    [Header("Tentacle Attack Settings")]
    [SerializeField] private float tentacleDamage;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float windupTime;
    [SerializeField] private float tentacleLifetime;
    [SerializeField] private LayerMask playerLayer;

    [Header("Prefabs")]
    [SerializeField] private GameObject tentaclePrefab;
    [SerializeField] private GameObject indicatorPrefab;

    [Header("Object Pools")]
    [SerializeField] private int poolSize;
    private Queue<GameObject> tentaclePool = new Queue<GameObject>();
    private Queue<GameObject> indicatorPool = new Queue<GameObject>();

    [Header("Attack Patterns")]
    public List<TentacleAttackPattern> attackPatterns;


    [Header("Debugging")]
    public int selectedPatternIndex = 0; // New variable to select a pattern

    private bool isAttacking = false;

    private void Start()
    {
        InitializePool(tentaclePool, tentaclePrefab, poolSize);
        InitializePool(indicatorPool, indicatorPrefab, poolSize);
        StartCoroutine(TentacleAttackRoutine());

        float multiplier = DifficultyManager.instance.GetCurrentEnemyDamageMultiplier();
        tentacleDamage *= multiplier;
    }

    private void InitializePool(Queue<GameObject> pool, GameObject prefab, int size)
    {
        for (int i = 0; i < size; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    private GameObject GetPooledObject(Queue<GameObject> pool, GameObject prefab)
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject obj = Instantiate(prefab);
            return obj;
        }
    }

    private void ReturnToPool(Queue<GameObject> pool, GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }

    private IEnumerator TentacleAttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackCooldown);
            if (!isAttacking && attackPatterns.Count > 0)
            {
                StartCoroutine(SpawnTentacles());
            }
        }
    }

    private IEnumerator SpawnTentacles()
    {
        isAttacking = true;

        TentacleAttackPattern selectedPattern = attackPatterns[Random.Range(0, attackPatterns.Count)];
        List<GameObject> indicators = new List<GameObject>();
        List<Vector2> storedPositions = new List<Vector2>();

        // Spawn indicators
        foreach (Vector2 localPosition in selectedPattern.tentaclePositions)
        {
            Vector2 worldPosition = (Vector2)transform.position + localPosition;
            storedPositions.Add(worldPosition);
            GameObject indicator = GetPooledObject(indicatorPool, indicatorPrefab);
            indicator.transform.position = worldPosition;
            indicators.Add(indicator);
        }

        yield return new WaitForSeconds(windupTime);

        List<GameObject> tentacles = new List<GameObject>();

        // Spawn tentacles
        foreach (Vector2 worldPosition in storedPositions)
        {
            GameObject tentacle = GetPooledObject(tentaclePool, tentaclePrefab);
            tentacle.transform.position = worldPosition;
            tentacles.Add(tentacle);

            // Apply damage
            Collider2D playerCollider = Physics2D.OverlapCircle(worldPosition, 0.5f, playerLayer);
            if (playerCollider)
            {
                playerCollider.GetComponentInParent<CharacterStatManager>()?.TakeDamage(tentacleDamage);
            }
        }

        // Remove indicators
        foreach (GameObject indicator in indicators)
        {
            ReturnToPool(indicatorPool, indicator);
        }

        // Wait before despawning tentacles
        yield return new WaitForSeconds(tentacleLifetime);

        // Return tentacles to the pool
        foreach (GameObject tentacle in tentacles)
        {
            ReturnToPool(tentaclePool, tentacle);
        }

        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPatterns == null || attackPatterns.Count == 0 || selectedPatternIndex < 0 || selectedPatternIndex >= attackPatterns.Count)
            return;

        Gizmos.color = Color.red;
        TentacleAttackPattern pattern = attackPatterns[selectedPatternIndex];

        foreach (Vector2 localPosition in pattern.tentaclePositions)
        {
            Vector2 worldPosition = (Vector2)transform.position + localPosition;
            Gizmos.DrawWireSphere(worldPosition, 0.3f);
        }
    }
}

[System.Serializable]
public class TentacleAttackPattern
{
    public string patternName;
    public List<Vector2> tentaclePositions;
}
