using UnityEngine;

public class Dart : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyStatManager enemy = collision.GetComponentInParent<EnemyStatManager>();
            if (enemy != null)
            {
                // Deal initial hit damage
                enemy.TakeDamage(PlayerUpgradeManager.Instance.blowDartStats.damage);

                // Apply or refresh poison
                enemy.StartPoison(
                    PlayerUpgradeManager.Instance.blowDartStats.poisonDamage,
                    2f, // min poison damage
                    5f, // poison duration
                    1f, // poison tick interval
                    Color.green // poison damage color
                );
            }

            // Disable dart visuals and collider
            spriteRenderer.enabled = false;
            boxCollider.enabled = false;
        }
    }
}
