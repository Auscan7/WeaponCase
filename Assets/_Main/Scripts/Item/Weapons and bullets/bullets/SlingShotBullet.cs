using UnityEngine;

public class SlingShotBullet : MonoBehaviour
{
    [Header("Rocket Damage Settings")]
    [SerializeField] float explosionRadius = 1f; // Radius of the explosion

    [Header("Explosion Effects")]
    [SerializeField] GameObject explosionEffectPrefab; // Optional visual effect for explosion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            CharacterStatManager target = collision.gameObject.GetComponentInParent<CharacterStatManager>();
            if (target != null)
            {
                target.TakeDamage(PlayerUpgradeManager.Instance.slingShotStats.damage);
                Explode(target); // Pass the target to exclude it later
            }

            Destroy(gameObject);
        }
    }

    private void Explode(CharacterStatManager excludeTarget)
    {
        AudioManager.instance.PlaySoundSFX(AudioManager.instance.rocketExplosionSFX);

        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

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
                enemyStats.TakeDamage(PlayerUpgradeManager.Instance.slingShotStats.areaDamage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the explosion radius in the editor for visualization
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
