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

    [Header("Projectile Attack Patterns")]
    public List<ProjectileAttackPattern> projectileAttackPatterns;

    [Header("Projectile Attack Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileDamage;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private int minProjectiles = 5;
    [SerializeField] private int maxProjectiles = 10;
    [SerializeField] private float projectileCooldown = 7f;  // how often this attack runs
    [SerializeField] private float projectileSpreadAngle = 90f; // spread in degrees

    private void Start()
    {
        InitializeProjectilePool(poolSize);  // same poolSize variable you already use
        StartCoroutine(ProjectileAttackRoutine());

        InitializePool(tentaclePool, tentaclePrefab, poolSize);
        InitializePool(indicatorPool, indicatorPrefab, poolSize);
        StartCoroutine(TentacleAttackRoutine());

        float multiplier = DifficultyManager.instance.GetCurrentEnemyDamageMultiplier();
        tentacleDamage *= multiplier;
    }

    private Queue<GameObject> projectilePool = new Queue<GameObject>();

    private void InitializeProjectilePool(int size)
    {
        for (int i = 0; i < size; i++)
        {
            GameObject obj = Instantiate(projectilePrefab);
            obj.SetActive(false);
            projectilePool.Enqueue(obj);
        }
    }

    private GameObject GetPooledProjectile()
    {
        if (projectilePool.Count > 0)
        {
            GameObject obj = projectilePool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject obj = Instantiate(projectilePrefab);
            return obj;
        }
    }

    private void ReturnProjectileToPool(GameObject obj)
    {
        obj.SetActive(false);
        projectilePool.Enqueue(obj);
    }

    private IEnumerator ProjectileAttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(projectileCooldown);

            if (!isAttacking)
            {
                StartCoroutine(ShootProjectiles());
            }
        }
    }

    private IEnumerator ShootProjectiles()
    {
        isAttacking = true;

        Vector2 bossPos = transform.position;

        // Pick a random projectile pattern
        ProjectileAttackPattern pattern = projectileAttackPatterns[Random.Range(0, projectileAttackPatterns.Count)];

        int projectileCount = pattern.projectileCount;

        switch (pattern.patternType)
        {
            case ProjectilePatternType.Circular:
                // Projectiles evenly spaced in circle with radius (spawn position offset)
                for (int i = 0; i < projectileCount; i++)
                {
                    float angle = (360f / projectileCount) * i;
                    Vector2 spawnPos = bossPos + new Vector2(
                        Mathf.Cos(angle * Mathf.Deg2Rad),
                        Mathf.Sin(angle * Mathf.Deg2Rad)
                    ) * pattern.radius;

                    ShootProjectile(spawnPos, new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)));
                }
                break;

            case ProjectilePatternType.Arc:
                // Projectiles spread evenly in arc (centered at random direction)
                float baseAngle = Random.Range(0f, 360f);
                float halfSpread = pattern.spreadAngle / 2f;

                for (int i = 0; i < projectileCount; i++)
                {
                    float angleStep = pattern.spreadAngle / (projectileCount - 1);
                    float angle = baseAngle - halfSpread + i * angleStep;

                    ShootProjectile(bossPos, new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)));
                }
                break;

            case ProjectilePatternType.RandomCrowded:
                // Spawn projectiles at boss position but shoot in random directions, clustered in one half circle
                float clusterCenter = Random.Range(0f, 360f);
                float clusterWidth = 90f; // degrees

                for (int i = 0; i < projectileCount; i++)
                {
                    float angle = clusterCenter - clusterWidth / 2f + Random.Range(0f, clusterWidth);
                    ShootProjectile(bossPos, new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)));
                }
                break;
        }

        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    private void ShootProjectile(Vector2 spawnPosition, Vector2 direction)
    {
        AudioManager.instance.PlaySoundSFX(AudioManager.instance.octopusProjectileSFX);
        GameObject proj = GetPooledProjectile();
        proj.transform.position = spawnPosition;

        var projectileMovement = proj.GetComponent<ProjectileMovement>();
        if (projectileMovement != null)
        {
            projectileMovement.SetDirection(direction, projectileSpeed, projectileDamage);
        }

        proj.SetActive(true);
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
            AudioManager.instance.PlaySoundSFX(AudioManager.instance.octopusTentacleSFX);
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

public enum ProjectilePatternType
{
    Circular,
    Arc,
    RandomCrowded
}

[System.Serializable]
public class ProjectileAttackPattern
{
    public ProjectilePatternType patternType;
    public int projectileCount;
    public float spreadAngle; // For Arc pattern
    public float radius;      // For Circular pattern
}