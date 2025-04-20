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
            // Apply damage to the enemy
            CharacterStatManager target = collision.gameObject.GetComponentInParent<CharacterStatManager>();
            if (target != null)
            {
                target.TakeDamage(PlayerUpgradeManager.Instance.slingShotStats.damage);
            }

            Explode();

            // Destroy the bullet
            Destroy(gameObject);
        }
    }

    private void Explode()
    {
        AudioManager.instance.PlaySoundSFX(AudioManager.instance.rocketExplosionSFX);

        // Instantiate explosion visual effect (if assigned)
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        // Modify the LayerMask to include multiple layers
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            transform.position,
            explosionRadius,
            LayerMask.GetMask("Enemy", "LanternFish", "Boss") // Add more layers as needed
        );

        // Apply damage to each enemy in the explosion radius
        foreach (Collider2D enemy in hitEnemies)
        {
            CharacterStatManager enemyStats = enemy.GetComponentInParent<CharacterStatManager>();
            if (enemyStats != null)
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
