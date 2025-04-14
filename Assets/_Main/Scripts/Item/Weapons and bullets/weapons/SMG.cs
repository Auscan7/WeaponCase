using UnityEngine;
using System.Collections;

public class SMG : Weapon
{
    public int bulletsPerBurst; // Number of bullets per burst
    public float burstFireRate; // Time between each bullet in burst
    public float spreadAngle; // Random bullet spread in degrees

    private void Start()
    {
        weaponRange = PlayerUpgradeManager.Instance.smgStats.range; // Set weapon range
    }

    public override void Fire(Vector2 targetPosition)
    {
        if (!CanFire()) return;

        // Set fire cooldown before burst starts to prevent multiple bursts firing at once
        SetNextFireTime(PlayerUpgradeManager.Instance.smgStats.firerate);

        WeaponCooldownUIManager.Instance.TriggerCooldown("SMG", PlayerUpgradeManager.Instance.smgStats.firerate);

        StartCoroutine(BurstFire(targetPosition));
    }

    private IEnumerator BurstFire(Vector2 targetPosition)
    {
        for (int i = 0; i < bulletsPerBurst; i++)
        {
            FireSingleShot(targetPosition);
            yield return new WaitForSeconds(burstFireRate); // Small delay between bullets            
        }
    }

    private void FireSingleShot(Vector2 targetPosition)
    {
        AudioManager.instance.PlaySoundSFX(AudioManager.instance.pistolFireSFX);

        Vector3 spawnPos = projectileSpawnPoint.position;

        // Calculate spread angle
        float randomAngle = Random.Range(-spreadAngle / 2, spreadAngle / 2);
        Vector2 direction = (targetPosition - (Vector2)spawnPos).normalized;
        direction = Quaternion.Euler(0, 0, randomAngle) * direction; // Apply spread

        // Spawn projectile
        GameObject projectile = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
        projectile.GetComponent<Rigidbody2D>().linearVelocity = direction * projectileSpeed;

        projectile.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

        EffectsManager.instance.PlayVFX(EffectsManager.instance.bulletVFX, spawnPos, Quaternion.identity);
    }
}
