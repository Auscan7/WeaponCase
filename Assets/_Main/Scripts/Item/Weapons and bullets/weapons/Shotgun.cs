using UnityEngine;

public class Shotgun : Weapon
{
    public float spreadAngle = 45f;

    [Header("Recoil Settings")]
    public Transform recoilTransform;
    private float recoilDistance = 1f;
    private float recoilResetSpeed = 25f;
    private float recoilHoldTime = 0.11f; // Time to pause at max recoil

    private Vector3 originalLocalPosition;
    private Vector3 recoilLocalPosition;

    private enum RecoilState { Idle, Recoiling, Holding, Returning }
    private RecoilState recoilState = RecoilState.Idle;

    private float holdTimer = 0f;

    private void Start()
    {
        weaponRange = PlayerUpgradeManager.Instance.shotgunStats.range; // Set the range from stats
        originalLocalPosition = recoilTransform.localPosition;
        recoilLocalPosition = originalLocalPosition - new Vector3(recoilDistance, 0f, 0f);
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
            recoilState = RecoilState.Recoiling;
            EffectsManager.instance.PlayVFX(EffectsManager.instance.pelletVFX, spawnPos, Quaternion.identity);
        }

        SetNextFireTime(fireRate);
    }
}
