using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    private Vector2 targetPosition;
    private float explosionRadius;
    private float speed;
    private bool hasExploded = false;
    [SerializeField] private LayerMask enemyLayers; // Allow multiple layers to be set in the Inspector

    public void SetTarget(Vector2 target, float radius, float grenadeSpeed)
    {
        targetPosition = target;
        explosionRadius = radius;
        speed = grenadeSpeed;
    }

    private void Update()
    {
        if (hasExploded) return;

        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            Explode();
        }
    }

    private void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;

        AudioManager.instance.PlaySoundSFX(AudioManager.instance.grenadeExplodeSFX);
        CameraShakeManager.Instance.Shake(0.2f, 0.05f);
        EffectsManager.instance.PlayVFX(EffectsManager.instance.grenadeVFX, transform.position, Quaternion.identity);

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.TryGetComponent(out CharacterStatManager enemyStats))
            {
                enemyStats.TakeDamage(PlayerUpgradeManager.Instance.grenadeStats.damage);
            }
        }

        Destroy(gameObject);
    }
}
