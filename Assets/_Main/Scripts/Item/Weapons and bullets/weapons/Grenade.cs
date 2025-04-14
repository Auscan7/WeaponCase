using UnityEngine;
using System.Collections;

public class Grenade : Weapon
{
    [Header("Grenade Settings")]
    private float throwDistance;
    private float explosionRadius = 2f;
    private float grenadeDamage;
    private float grenadeSpeed = 5f;

    [SerializeField] private GameObject explosionEffectPrefab;
    private bool isFiring = false;

    private void Start()
    {
        throwDistance = PlayerUpgradeManager.Instance.grenadeStats.range;
        grenadeDamage = PlayerUpgradeManager.Instance.grenadeStats.damage;

        StartCoroutine(GrenadeThrowRoutine());
    }

    private IEnumerator GrenadeThrowRoutine()
    {
        isFiring = true;

        while (true) // Keep throwing grenades at intervals
        {
            Fire(transform.position); // Use player's position as reference
            yield return new WaitForSeconds(PlayerUpgradeManager.Instance.grenadeStats.firerate);
        }
    }

    public override void Fire(Vector2 targetPosition)
    {
        if (!CanFire()) return;

        WeaponCooldownUIManager.Instance.TriggerCooldown("Grenade", PlayerUpgradeManager.Instance.grenadeStats.firerate);

        float angleStep = 360f / PlayerUpgradeManager.Instance.baseGrenadeProjectileCount;
        float currentAngle = 0f;

        for (int i = 0; i < PlayerUpgradeManager.Instance.baseGrenadeProjectileCount; i++)
        {
            Vector2 direction = new Vector2(Mathf.Cos(currentAngle * Mathf.Deg2Rad), Mathf.Sin(currentAngle * Mathf.Deg2Rad));
            Vector2 grenadeTarget = (Vector2)transform.position + direction * throwDistance;

            GameObject grenade = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            GrenadeProjectile grenadeScript = grenade.GetComponent<GrenadeProjectile>();
            if (grenadeScript != null)
            {
                grenadeScript.SetTarget(grenadeTarget, explosionRadius, grenadeDamage, grenadeSpeed);
            }

            currentAngle += angleStep;
        }

        SetNextFireTime(PlayerUpgradeManager.Instance.grenadeStats.firerate);
    }
}
