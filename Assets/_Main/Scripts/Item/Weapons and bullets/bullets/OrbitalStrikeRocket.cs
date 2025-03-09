using UnityEngine;

public class OrbitalStrikeRocket : MonoBehaviour
{
    [Header("Rocket Damage Settings")]
    [SerializeField] float explosionRadius = 1f; // Radius of the explosion
    [SerializeField] private LayerMask enemyLayers; // Allow multiple layers to be set in the Inspector

    [Header("Explosion Effects")]
    [SerializeField] GameObject explosionEffectPrefab; // Optional visual effect for explosion

    // Removed OnTriggerEnter2D so that the rocket doesn't explode on enemy collision

    public void Explode()
    {
        AudioManager.instance.PlaySoundSFX(AudioManager.instance.rocketExplosionSFX);

        // Instantiate explosion visual effect if assigned
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        // Detect enemies within the explosion radius and apply the same damage to all
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            CharacterStatManager enemyStats = enemy.GetComponentInParent<CharacterStatManager>();
            if (enemyStats != null)
            {
                // Apply the same damage to each enemy
                enemyStats.TakeDamage(PlayerUpgradeManager.Instance.rocketStats.damage);
            }
        }

        // Destroy the rocket after explosion
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the explosion radius for visualization in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
