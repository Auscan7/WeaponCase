using UnityEngine;

public class Rocket : MonoBehaviour
{
    [Header("Rocket Damage Settings")]
    [SerializeField] float explosionRadius = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            CharacterStatManager target = collision.gameObject.GetComponentInParent<CharacterStatManager>();
            if (target != null)
            {
                target.TakeDamage(PlayerUpgradeManager.Instance.rocketStats.damage);
                Explode(target); // Pass the target to exclude from area damage
            }

            Destroy(gameObject);
        }
    }

    private void Explode(CharacterStatManager excludeTarget)
    {
        CameraShakeManager.Instance.Shake(0.3f, 0.05f);
        AudioManager.instance.PlaySoundSFX(AudioManager.instance.rocketExplosionSFX);
        EffectsManager.instance.PlayVFX(EffectsManager.instance.rocketVFX, transform.position, Quaternion.identity);

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            transform.position,
            explosionRadius,
            LayerMask.GetMask("Enemy", "LanternFish", "Boss")
        );

        foreach (Collider2D enemy in hitEnemies)
        {
            CharacterStatManager enemyStats = enemy.GetComponentInParent<CharacterStatManager>();
            if (enemyStats != null && enemyStats != excludeTarget)
            {
                enemyStats.TakeDamage(PlayerUpgradeManager.Instance.rocketStats.areaDamage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
