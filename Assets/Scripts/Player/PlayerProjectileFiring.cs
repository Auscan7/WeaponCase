using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileFiring : MonoBehaviour
{
    [Header("Weapon Prefabs")]
    public GameObject bulletPrefab;
    public GameObject rocketPrefab;
    public GameObject pelletPrefab;

    [Header("General Settings")]
    public float detectionRange = 10f; // Range to detect enemies
    public float fireRate = 1f; // Bullets per second
    public float projectileSpeed = 10f;
    public Transform projectileSpawnPoint;

    [Header("Shotgun Settings")]
    public int pelletCount = 5;
    public float spreadAngle = 45f;

    private float nextFireTime = 0f;
    private bool isEnemyInRange = false;

    private void Awake()
    {

    }

    void Update()
    {
        DetectAndFireAtEnemy();
    }

    private void DetectAndFireAtEnemy()
    {
        // Detect all enemies within range
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, detectionRange, LayerMask.GetMask("Enemy"));

        if (enemiesInRange.Length > 0)
        {
            // Mark that an enemy is detected
            isEnemyInRange = true;

            // Find the closest enemy
            Collider2D closestEnemy = GetClosestEnemy(enemiesInRange);

            // Try to fire at the closest enemy
            if (Time.time >= nextFireTime)
            {
                FireShotgun(closestEnemy.transform.position);
                FireBullet(closestEnemy.transform.position);
                FireRocket(closestEnemy.transform.position);

                nextFireTime = Time.time + 1f / fireRate; // Set next fire time
            }
        }
        else
        {
            // No enemies detected
            isEnemyInRange = false;
        }
    }

    private Collider2D GetClosestEnemy(Collider2D[] enemies)
    {
        Collider2D closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider2D enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = enemy;
            }
        }

        return closest;
    }

    private void FireBullet(Vector2 targetPosition)
    {
        if (bulletPrefab != null)
        {
            // Instantiate projectile
            Vector3 spawnPosition = projectileSpawnPoint != null ? projectileSpawnPoint.position : transform.position;
            GameObject projectile = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);

            // Calculate direction to target
            Vector2 direction = (targetPosition - (Vector2)spawnPosition).normalized;

            // Set projectile velocity
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = direction * projectileSpeed;
            }

            // Rotate projectile to face target
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void FireRocket(Vector2 targetPosition)
    {
        if (rocketPrefab != null)
        {
            // Instantiate projectile
            Vector3 spawnPosition = projectileSpawnPoint != null ? projectileSpawnPoint.position : transform.position;
            GameObject projectile = Instantiate(rocketPrefab, spawnPosition, Quaternion.identity);

            // Calculate direction to target
            Vector2 direction = (targetPosition - (Vector2)spawnPosition).normalized;

            // Set projectile velocity
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = direction * projectileSpeed;
            }

            // Rotate projectile to face target
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void FireShotgun(Vector2 targetPosition)
    {
        if (pelletPrefab != null)
        {
            // Spawn point for projectiles
            Vector3 spawnPosition = projectileSpawnPoint != null ? projectileSpawnPoint.position : transform.position;

            // Calculate the base direction to the target
            Vector2 baseDirection = (targetPosition - (Vector2)spawnPosition).normalized;

            // Calculate the angle for the center projectile
            float baseAngle = Mathf.Atan2(baseDirection.y, baseDirection.x) * Mathf.Rad2Deg;

            // Spread the bullets evenly across the spread angle
            float halfSpread = spreadAngle / 2f;
            float angleIncrement = spreadAngle / (pelletCount - 1);

            for (int i = 0; i < pelletCount; i++)
            {
                // Calculate the angle for this projectile
                float currentAngle = baseAngle - halfSpread + (angleIncrement * i);
                Vector2 direction = new Vector2(
                    Mathf.Cos(currentAngle * Mathf.Deg2Rad),
                    Mathf.Sin(currentAngle * Mathf.Deg2Rad)
                ).normalized;

                // Instantiate the projectile
                GameObject projectile = Instantiate(pelletPrefab, spawnPosition, Quaternion.identity);

                // Set projectile velocity
                Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.velocity = direction * projectileSpeed;
                }

                // Rotate projectile to face its direction
                projectile.transform.rotation = Quaternion.Euler(0, 0, currentAngle);
            }
        }
    }

    private void OnDrawGizmos()
    {
        // Draw detection range in the editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Optionally visualize the "isEnemyInRange" state
        if (isEnemyInRange)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectionRange * 0.5f);
        }
    }
}