using UnityEngine;

public class Pistol : Weapon
{
    [Header("Recoil Settings")]
    public Transform recoilTransform;
    private float recoilDistance = 1.1f;
    private float recoilResetSpeed = 50f;
    private float recoilHoldTime = 0.09f; // Time to pause at max recoil

    private Vector3 originalLocalPosition;
    private Vector3 recoilLocalPosition;

    private enum RecoilState { Idle, Recoiling, Holding, Returning }
    private RecoilState recoilState = RecoilState.Idle;

    private float holdTimer = 0f;

    private void Start()
    {
        weaponRange = PlayerUpgradeManager.Instance.pistolStats.range;
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

        float fireRate = PlayerUpgradeManager.Instance.pistolStats.firerate;
        WeaponCooldownUIManager.Instance.TriggerCooldown("Pistol", fireRate);
        AudioManager.instance.PlaySoundSFX(AudioManager.instance.pistolFireSFX);

        Vector3 spawnPos = projectileSpawnPoint.position;
        GameObject projectile = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);

        Vector2 direction = (targetPosition - (Vector2)spawnPos).normalized;
        projectile.GetComponent<Rigidbody2D>().linearVelocity = direction * projectileSpeed;
        projectile.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

        EffectsManager.instance.PlayVFX(EffectsManager.instance.bulletVFX, spawnPos, Quaternion.identity);

        recoilState = RecoilState.Recoiling;

        SetNextFireTime(fireRate);
    }
}
