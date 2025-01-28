using UnityEngine;

public class VortexDamage : MonoBehaviour
{
    private float damage = 1f;
    private float damageInterval = 1f;

    private float damageCooldown;

    private void Update()
    {
        if (damageCooldown > 0)
        {
            damageCooldown -= Time.deltaTime;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && damageCooldown <= 0)
        {
            collision.gameObject.GetComponentInParent<CharacterStatManager>().TakeDamage(damage);

            damageCooldown = damageInterval;
        }
    }
}
