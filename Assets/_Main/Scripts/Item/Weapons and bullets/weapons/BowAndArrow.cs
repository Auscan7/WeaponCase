using UnityEngine;

public class BowAndArrow : Weapon
{
    private void Start()
    {
        weaponRange = PlayerUpgradeManager.Instance.bowAndArrowStats.range; // Set the range from stats
    }

    public override void Fire(Vector2 targetPosition)
    {
        if (!CanFire()) return;
        float fireRate = PlayerUpgradeManager.Instance.bowAndArrowStats.firerate;
        WeaponCooldownUIManager.Instance.TriggerCooldown("BowAndArrow", fireRate);

        AudioManager.instance.PlaySoundSFX(AudioManager.instance.bowAndArrowFireSFX);

        Vector3 spawnPos = projectileSpawnPoint.position;
        GameObject projectile = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);

        Vector2 direction = (targetPosition - (Vector2)spawnPos).normalized;
        projectile.GetComponent<Rigidbody2D>().linearVelocity = direction * projectileSpeed;

        projectile.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

        EffectsManager.instance.PlayVFX(EffectsManager.instance.bulletVFX, spawnPos, Quaternion.identity);

        SetNextFireTime(fireRate);
    }
}
