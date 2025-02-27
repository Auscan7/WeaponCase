using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OrbitalStrike : Weapon
{
    public GameObject crosshairPrefab; // Crosshair indicator prefab
    public float spawnHeight = 10f; // Height rockets spawn from
    public int rocketsPerCluster = 5; // Rockets per cluster
    public float rocketSpawnDelay = 0.2f; // Delay between each rocket in a cluster
    public float targetOffsetRange = 1.5f; // Random offset range

    private bool isFiring = false;

    private void Start()
    {
        weaponRange = PlayerUpgradeManager.Instance.orbitalStrikeStats.range; // Set weapon range
    }

    public override void Fire(Vector2 targetPosition)
    {
        if (!CanFire() || isFiring) return;

        SetNextFireTime(PlayerUpgradeManager.Instance.orbitalStrikeStats.firerate);
        StartCoroutine(OrbitalStrikeRoutine());
    }

    private IEnumerator OrbitalStrikeRoutine()
    {
        isFiring = true;

        while (true) // Keep firing clusters
        {
            List<Transform> enemiesInRange = GetEnemiesInRange();
            if (enemiesInRange.Count > 0)
            {
                yield return StartCoroutine(FireCluster(enemiesInRange));
            }

            yield return new WaitForSeconds(PlayerUpgradeManager.Instance.orbitalStrikeStats.firerate);
        }
    }

    private IEnumerator FireCluster(List<Transform> enemies)
    {
        for (int i = 0; i < rocketsPerCluster; i++)
        {
            if (enemies.Count == 0) yield break;

            Transform target = enemies[Random.Range(0, enemies.Count)];
            Vector2 targetPos = target.position;
            Vector2 offset = new Vector2(Random.Range(-targetOffsetRange, targetOffsetRange), Random.Range(-targetOffsetRange, targetOffsetRange));
            Vector2 finalTargetPos = targetPos + offset;

            // Spawn crosshair at the intended impact spot
            GameObject crosshair = Instantiate(crosshairPrefab, finalTargetPos, Quaternion.identity);

            yield return new WaitForSeconds(rocketSpawnDelay / 2f); // Delay before rocket spawns

            SpawnRocket(finalTargetPos, crosshair);

            yield return new WaitForSeconds(rocketSpawnDelay);
        }
    }

    private void SpawnRocket(Vector2 targetPosition, GameObject crosshair)
    {
        Vector2 spawnPosition = new Vector2(targetPosition.x, targetPosition.y + spawnHeight);

        // Create the rocket and set its rotation to face downward (-90 degrees on Z-axis)
        GameObject rocket = Instantiate(projectilePrefab, spawnPosition, Quaternion.Euler(0, 0, -90));

        StartCoroutine(MoveRocket(rocket, targetPosition, crosshair));
    }

    private IEnumerator MoveRocket(GameObject rocket, Vector2 targetPosition, GameObject crosshair)
    {
        while (rocket != null && (Vector2)rocket.transform.position != targetPosition)
        {
            rocket.transform.position = Vector2.MoveTowards(rocket.transform.position, targetPosition, projectileSpeed * Time.deltaTime);
            yield return null;
        }

        // When the rocket reaches the target, call its Explode() method
        OrbitalStrikeRocket rocketScript = rocket.GetComponent<OrbitalStrikeRocket>();
        if (rocketScript != null)
        {
            rocketScript.Explode();
        }
        else
        {
            // Fallback explosion effect if no rocket script found
            //EffectsManager.instance.PlayVFX(EffectsManager.instance.explosionVFX, targetPosition, Quaternion.identity);
            Destroy(rocket);
        }

        Destroy(crosshair);
    }

    private List<Transform> GetEnemiesInRange()
    {
        List<Transform> enemyTransforms = new List<Transform>();

        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, weaponRange, LayerMask.GetMask("Enemy"));
        foreach (Collider2D enemy in enemies)
        {
            enemyTransforms.Add(enemy.transform);
        }

        return enemyTransforms;
    }
}
