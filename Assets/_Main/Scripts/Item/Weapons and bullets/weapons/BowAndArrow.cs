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
        Vector2 velocity = direction * projectileSpeed;

        // Initialize arrow with velocity, hop count, and hop range
        Arrow arrow = projectile.GetComponent<Arrow>();
        arrow.Initialize(velocity, hops: PlayerUpgradeManager.Instance.baseBowAndArrowHopCount, range: 5f);

        projectile.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

        EffectsManager.instance.PlayVFX(EffectsManager.instance.bulletVFX, spawnPos, Quaternion.identity);

        SetNextFireTime(fireRate);
    }
}
