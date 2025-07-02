using UnityEngine;
using System.Collections;

public class PoisonCloud : MonoBehaviour
{
    [SerializeField] private float tickInterval = 1f;
    [SerializeField] private float defaultDuration = 5f;
    [SerializeField] private float damagePerTick = 2f;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private ParticleSystem poisonParticles;

    private Coroutine damageRoutine;

    public void Initialize(float damage, float duration)
    {
        damagePerTick = damage;
        defaultDuration = duration;

        if (poisonParticles != null)
        {
            // Stop and fully clear before modifying
            poisonParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

            var main = poisonParticles.main;
            main.duration = defaultDuration;

            poisonParticles.Play();
        }

        damageRoutine = StartCoroutine(DamageRoutine());
    }

    private IEnumerator DamageRoutine()
    {
        float elapsed = 0f;
        while (elapsed < defaultDuration)
        {
            DamageEnemiesInRadius();
            yield return new WaitForSeconds(tickInterval);
            elapsed += tickInterval;
        }

        if (poisonParticles != null)
        {
            poisonParticles.Stop();
        }

        yield return new WaitForSeconds(1f); // give time for particle fade out if needed
        Destroy(gameObject);
    }

    private void DamageEnemiesInRadius()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, 1.5f, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.TryGetComponent(out CharacterStatManager stats))
            {
                float damageBasedOnMaxHP = stats.maxHealth * 0.015f; // 2% of max HP
                float finalDamage = Mathf.Max(damagePerTick, damageBasedOnMaxHP);
                stats.TakeDamage(finalDamage, Color.green);
            }
        }
    }
}
