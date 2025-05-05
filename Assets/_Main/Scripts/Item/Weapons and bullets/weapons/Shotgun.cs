using UnityEngine;

public class Shotgun : Weapon
{
    public float spreadAngle = 45f;

    private void Start()
    {
        weaponRange = PlayerUpgradeManager.Instance.shotgunStats.range; // Set the range from stats
    }

    public override void Fire(Vector2 targetPosition)
    {
        if (!CanFire()) return;
        float fireRate = PlayerUpgradeManager.Instance.shotgunStats.firerate;
        WeaponCooldownUIManager.Instance.TriggerCooldown("Shotgun", fireRate);

        AudioManager.instance.PlaySoundSFX(AudioManager.instance.shotgunFireSFX);

        Vector3 spawnPos = projectileSpawnPoint.position;
        Vector2 baseDirection = (targetPosition - (Vector2)spawnPos).normalized;
        float baseAngle = Mathf.Atan2(baseDirection.y, baseDirection.x) * Mathf.Rad2Deg;
        float halfSpread = spreadAngle / 2f;
        float angleStep = spreadAngle / (PlayerUpgradeManager.Instance.baseShotgunProjectileCount - 1);

        for (int i = 0; i < PlayerUpgradeManager.Instance.baseShotgunProjectileCount; i++)
        {
            float currentAngle = baseAngle - halfSpread + (angleStep * i);
            Vector2 direction = new Vector2(Mathf.Cos(currentAngle * Mathf.Deg2Rad), Mathf.Sin(currentAngle * Mathf.Deg2Rad)).normalized;

            GameObject projectile = Instantiate(projectilePrefab, spawnPos, Quaternion.Euler(0, 0, currentAngle));
            projectile.GetComponent<Rigidbody2D>().linearVelocity = direction * projectileSpeed;

            EffectsManager.instance.PlayVFX(EffectsManager.instance.pelletVFX, spawnPos, Quaternion.identity);
        }

        SetNextFireTime(fireRate);
    }
}
