using UnityEngine;

public class Rocket : MonoBehaviour
{
    [Header("Rocket Damage Settings")]
    [SerializeField] float explosionRadius = 1f; // Radius of the explosion

    [Header("Explosion Effects")]
    [SerializeField] GameObject explosionEffectPrefab; // Optional visual effect for explosion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Apply direct hit damage
            CharacterStatManager target = collision.gameObject.GetComponentInParent<CharacterStatManager>();
            if (target != null)
            {
                target.TakeDamage(PlayerUpgradeManager.Instance.rocketStats.damage);
            }

            // Trigger the explosion
            Explode();

            // Destroy the rocket
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

        // Detect enemies within the explosion radius
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, explosionRadius, LayerMask.GetMask("Enemy"));

        // Apply damage to each enemy in the explosion radius
        foreach (Collider2D enemy in hitEnemies)
        {
            CharacterStatManager enemyStats = enemy.GetComponentInParent<CharacterStatManager>();
            if (enemyStats != null)
            {
                enemyStats.TakeDamage(PlayerUpgradeManager.Instance.rocketStats.areaDamage);
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
