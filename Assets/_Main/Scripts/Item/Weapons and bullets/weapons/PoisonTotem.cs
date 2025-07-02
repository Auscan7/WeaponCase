using UnityEngine;
using System.Collections;

public class PoisonTotem : Weapon
{
    private void Start()
    {
        StartCoroutine(SpawnCloudRoutine());
    }

    private IEnumerator SpawnCloudRoutine()
    {

        while (true)
        {
            float firerate = PlayerUpgradeManager.Instance.poisonTotemStats.firerate;
            Fire(transform.position);
            yield return new WaitForSeconds(1f / firerate);
        }
    }

    public override void Fire(Vector2 targetPosition)
    {
        if (!CanFire()) return;

        float firerate = PlayerUpgradeManager.Instance.poisonTotemStats.firerate;

        WeaponCooldownUIManager.Instance.TriggerCooldown("PoisonTotem", firerate);
        AudioManager.instance.PlaySoundSFX(AudioManager.instance.poisonTotemSFX);

        GameObject cloud = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        PoisonCloud poisonScript = cloud.GetComponent<PoisonCloud>();
        if (poisonScript != null)
        {
            poisonScript.Initialize(PlayerUpgradeManager.Instance.poisonTotemStats.damage, PlayerUpgradeManager.Instance.poisonTotemStats.duration);
        }

        SetNextFireTime(firerate);
    }

    public override void RotateTowardsTarget(Vector2 targetPosition)
    {
    }
}
