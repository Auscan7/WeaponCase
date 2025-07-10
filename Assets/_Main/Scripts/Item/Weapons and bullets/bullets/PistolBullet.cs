using UnityEngine;

public class PistolBullet : MonoBehaviour
{
    private int maxPierceCount;
    private int currentPierceCount = 0;

    private void Start()
    {
        // Load from upgrade manager
        maxPierceCount = PlayerUpgradeManager.Instance.basePistolPierceCount;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Transform hitTarget = collision.transform;

            collision.GetComponentInParent<CharacterStatManager>()?.TakeDamage(PlayerUpgradeManager.Instance.pistolStats.damage);
            EffectsManager.instance.PlayVFX(EffectsManager.instance.bulletHitVFX, collision.transform.position, Quaternion.identity);

            currentPierceCount++;

            if (currentPierceCount > maxPierceCount)
            {
                Destroy(gameObject);
            }
        }
    }
}
