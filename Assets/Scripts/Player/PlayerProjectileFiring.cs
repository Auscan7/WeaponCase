using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileFiring : MonoBehaviour
{
    public float detectionRange; // Range to detect enemies
    public float fireRate = 1f; // Bullets per second
    public GameObject projectilePrefab; // Projectile prefab
    public Transform projectileSpawnPoint; // Spawn point for projectiles
    public float projectileSpeed = 10f; // Speed of the projectile

    private float nextFireTime = 0f; // Time for the next shot
    private bool isEnemyInRange = false; // Whether an enemy is in range

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
                FireProjectile(closestEnemy.transform.position);
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

    private void FireProjectile(Vector2 targetPosition)
    {
        // Instantiate projectile
        Vector3 spawnPosition = projectileSpawnPoint != null ? projectileSpawnPoint.position : transform.position;
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

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
