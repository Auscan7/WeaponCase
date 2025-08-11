using UnityEngine;
using System.Collections;

public class RocketLauncher : Weapon
{
    public Transform secondaryProjectileSpawnPoint; // assign in inspector

    private void Start()
    {
        weaponRange = PlayerUpgradeManager.Instance.rocketStats.range;
    }

    public override void Fire(Vector2 targetPosition)
    {
        if (!CanFire()) return;

        float firerate = PlayerUpgradeManager.Instance.rocketStats.firerate;
        WeaponCooldownUIManager.Instance.TriggerCooldown("Rocket", firerate);
        AudioManager.instance.PlaySoundSFX(AudioManager.instance.rocketFireSFX);

        // Fire first rocket immediately
        FireRocket(projectileSpawnPoint, targetPosition);

        // Fire second rocket with delay
        StartCoroutine(FireDelayedRocket(targetPosition, 0.1f));

        // Set fire cooldown after second rocket
        SetNextFireTime(firerate);
    }

    private void FireRocket(Transform spawnPoint, Vector2 targetPosition)
    {
        Vector3 spawnPos = spawnPoint.position;
        GameObject projectile = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);

        Vector2 direction = (targetPosition - (Vector2)spawnPos).normalized;
        projectile.GetComponent<Rigidbody2D>().linearVelocity = direction * projectileSpeed;
        projectile.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
    }

    private IEnumerator FireDelayedRocket(Vector2 targetPosition, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (secondaryProjectileSpawnPoint != null)
        {
            FireRocket(secondaryProjectileSpawnPoint, targetPosition);
            AudioManager.instance.PlaySoundSFX(AudioManager.instance.rocketFireSFX); // Optional: play SFX again
        }
    }
}
