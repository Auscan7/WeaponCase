using UnityEngine;

public class RocketLauncher : Weapon
{
    private void Start()
    {
        weaponRange = PlayerUpgradeManager.Instance.rocketStats.range; // Set the range from stats
    }

    public override void Fire(Vector2 targetPosition)
    {
        if (!CanFire()) return;

        float firerate = PlayerUpgradeManager.Instance.rocketStats.firerate;
        WeaponCooldownUIManager.Instance.TriggerCooldown("Rocket", firerate);

        AudioManager.instance.PlaySoundSFX(AudioManager.instance.rocketFireSFX);

        Vector3 spawnPos = projectileSpawnPoint.position;
        GameObject projectile = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);

        Vector2 direction = (targetPosition - (Vector2)spawnPos).normalized;
        projectile.GetComponent<Rigidbody2D>().linearVelocity = direction * projectileSpeed;

        projectile.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

        SetNextFireTime(firerate);
    }
}
