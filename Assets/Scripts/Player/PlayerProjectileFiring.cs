using UnityEngine;

public class PlayerProjectileFiring : MonoBehaviour
{
    [Header("Weapon Prefabs")]
    public GameObject bulletPrefab;
    public GameObject rocketPrefab;
    public GameObject pelletPrefab;

    [Header("General Settings")]
    public float detectionRange = 10f; // Range to detect enemies
    public float projectileSpeed = 10f;
    public Transform projectileSpawnPoint;

    [Header("Shotgun Settings")]
    public int pelletCount = 5;
    public float spreadAngle = 45f;

    private float nextPistolFireTime = 0f;
    private float nextShotgunFireTime = 0f;
    private float nextRocketFireTime = 0f;

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
            // Find the closest enemy
            Collider2D closestEnemy = GetClosestEnemy(enemiesInRange);

            // Fire pistol if conditions are met
            if (Time.time >= nextPistolFireTime)
            {
                FireBullet(closestEnemy.transform.position);
                nextPistolFireTime = Time.time + 1f / UpgradeManager.Instance.pistolFirerate;
            }

            // Fire shotgun if conditions are met
            if (Time.time >= nextShotgunFireTime)
            {
                FireShotgun(closestEnemy.transform.position);
                nextShotgunFireTime = Time.time + 1f / UpgradeManager.Instance.shotgunFirerate;
            }

            // Fire rocket if conditions are met
            if (Time.time >= nextRocketFireTime)
            {
                FireRocket(closestEnemy.transform.position);
                nextRocketFireTime = Time.time + 1f / UpgradeManager.Instance.rocketFirerate;
            }
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
            AudioManager.instance.PlaySoundSFX(AudioManager.instance.pistolFireSFX);

            Vector3 spawnPosition = projectileSpawnPoint != null ? projectileSpawnPoint.position : transform.position;
            GameObject projectile = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);

            Vector2 direction = (targetPosition - (Vector2)spawnPosition).normalized;

            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = direction * projectileSpeed;
            }

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.Euler(0, 0, angle);

            EffectsManager.instance.PlayVFX(EffectsManager.instance.bulletVFX, spawnPosition, Quaternion.Euler(-angle, 90, 0));
        }
    }

    private void FireRocket(Vector2 targetPosition)
    {
        if (rocketPrefab != null)
        {
            AudioManager.instance.PlaySoundSFX(AudioManager.instance.rocketFireSFX);

            Vector3 spawnPosition = projectileSpawnPoint != null ? projectileSpawnPoint.position : transform.position;
            GameObject projectile = Instantiate(rocketPrefab, spawnPosition, Quaternion.identity);

            Vector2 direction = (targetPosition - (Vector2)spawnPosition).normalized;

            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = direction * projectileSpeed;
            }

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void FireShotgun(Vector2 targetPosition)
    {
        if (pelletPrefab != null)
        {
            AudioManager.instance.PlaySoundSFX(AudioManager.instance.shotgunFireSFX);

            Vector3 spawnPosition = projectileSpawnPoint != null ? projectileSpawnPoint.position : transform.position;
            Vector2 baseDirection = (targetPosition - (Vector2)spawnPosition).normalized;

            float baseAngle = Mathf.Atan2(baseDirection.y, baseDirection.x) * Mathf.Rad2Deg;
            float halfSpread = spreadAngle / 2f;
            float angleIncrement = spreadAngle / (pelletCount - 1);

            for (int i = 0; i < pelletCount; i++)
            {
                float currentAngle = baseAngle - halfSpread + (angleIncrement * i);
                Vector2 direction = new Vector2(
                    Mathf.Cos(currentAngle * Mathf.Deg2Rad),
                    Mathf.Sin(currentAngle * Mathf.Deg2Rad)
                ).normalized;

                GameObject projectile = Instantiate(pelletPrefab, spawnPosition, Quaternion.identity);

                Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.velocity = direction * projectileSpeed;
                }

                projectile.transform.rotation = Quaternion.Euler(0, 0, currentAngle);

                EffectsManager.instance.PlayVFX(EffectsManager.instance.pelletVFX, spawnPosition, Quaternion.Euler(-currentAngle, 90, 0));
            }
        }
    }
}
