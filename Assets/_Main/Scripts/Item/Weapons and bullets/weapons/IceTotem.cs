using UnityEngine;
using System.Collections;

public class IceTotem : Weapon
{
    private float projectileSpeed = 8f;
    private bool isFiring = false;

    private void Start()
    {
        StartCoroutine(FiringRoutine());
    }

    private IEnumerator FiringRoutine()
    {
        isFiring = true;
        float firerate = PlayerUpgradeManager.Instance.iceTotemStats.firerate;

        while (true)
        {
            Fire(transform.position);
            yield return new WaitForSeconds(1 / firerate);
        }
    }

    public override void Fire(Vector2 origin)
    {
        if (!CanFire()) return;

        float firerate = PlayerUpgradeManager.Instance.iceTotemStats.firerate;
        WeaponCooldownUIManager.Instance.TriggerCooldown("IceTotem", firerate);
        AudioManager.instance.PlaySoundSFX(AudioManager.instance.iceTotemSFX);

        float angleStep = 360f / PlayerUpgradeManager.Instance.baseIceTotemProjectileCount;
        float currentAngle = 0f;

        for (int i = 0; i < PlayerUpgradeManager.Instance.baseIceTotemProjectileCount; i++)
        {
            Vector2 direction = new Vector2(Mathf.Cos(currentAngle * Mathf.Deg2Rad), Mathf.Sin(currentAngle * Mathf.Deg2Rad));

            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            IceTotemProjectile iceScript = projectile.GetComponent<IceTotemProjectile>();
            if (iceScript != null)
            {
                iceScript.Initialize(direction, projectileSpeed);
            }

            currentAngle += angleStep;
        }

        SetNextFireTime(firerate);
    }

    public override void RotateTowardsTarget(Vector2 targetPosition)
    {

    }
}
