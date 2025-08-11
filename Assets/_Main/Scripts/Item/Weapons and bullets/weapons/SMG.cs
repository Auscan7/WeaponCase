using UnityEngine;
using System.Collections;

public class SMG : Weapon
{
    [Header("Recoil Settings")]
    public Transform recoilTransform;
    private float recoilDistance = 0.4f;
    private float recoilResetSpeed = 100f;
    private float recoilHoldTime = 0.01f; // Time to pause at max recoil

    private Vector3 originalLocalPosition;
    private Vector3 recoilLocalPosition;

    private enum RecoilState { Idle, Recoiling, Holding, Returning }
    private RecoilState recoilState = RecoilState.Idle;

    private float holdTimer = 0f;

    public int bulletsPerBurst; // Number of bullets per burst
    public float burstFireRate; // Time between each bullet in burst
    public float spreadAngle; // Random bullet spread in degrees

    private void Start()
    {
        weaponRange = PlayerUpgradeManager.Instance.smgStats.range; // Set weapon range
        originalLocalPosition = recoilTransform.localPosition;
        recoilLocalPosition = originalLocalPosition - new Vector3(recoilDistance, 0f, 0f);
    }

    public override void Fire(Vector2 targetPosition)
    {
        if (!CanFire()) return;
        AudioManager.instance.PlaySoundSFX(AudioManager.instance.smgFireSFX);
        float fireRate = PlayerUpgradeManager.Instance.smgStats.firerate;

        // Set fire cooldown before burst starts to prevent multiple bursts firing at once
        SetNextFireTime(fireRate);

        WeaponCooldownUIManager.Instance.TriggerCooldown("SMG", fireRate);

        StartCoroutine(BurstFire(targetPosition));
    }

    private void Update()
    {
        if (recoilTransform == null) return;

        switch (recoilState)
        {
            case RecoilState.Recoiling:
                recoilTransform.localPosition = Vector3.Lerp(
                    recoilTransform.localPosition,
                    recoilLocalPosition,
                    Time.deltaTime * recoilResetSpeed
                );

                if (Vector3.Distance(recoilTransform.localPosition, recoilLocalPosition) < 0.001f)
                {
                    recoilTransform.localPosition = recoilLocalPosition;
                    holdTimer = recoilHoldTime;
                    recoilState = RecoilState.Holding;
                }
                break;

            case RecoilState.Holding:
                holdTimer -= Time.deltaTime;
                if (holdTimer <= 0f)
                {
                    recoilState = RecoilState.Returning;
                }
                break;

            case RecoilState.Returning:
                recoilTransform.localPosition = Vector3.Lerp(
                    recoilTransform.localPosition,
                    originalLocalPosition,
                    Time.deltaTime * recoilResetSpeed
                );

                if (Vector3.Distance(recoilTransform.localPosition, originalLocalPosition) < 0.001f)
                {
                    recoilTransform.localPosition = originalLocalPosition;
                    recoilState = RecoilState.Idle;
                }
                break;
        }
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
        recoilState = RecoilState.Recoiling;
    }
}
