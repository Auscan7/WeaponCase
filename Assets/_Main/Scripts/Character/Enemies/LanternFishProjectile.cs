using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternFishProjectile : MonoBehaviour
{
    public float damage;

    private void Start()
    {
        float multiplier = DifficultyManager.instance.GetCurrentEnemyDamageMultiplier();
        damage *= multiplier;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponentInParent<CharacterStatManager>().TakeDamage(damage);

            Destroy(gameObject);
        }
    }
}
