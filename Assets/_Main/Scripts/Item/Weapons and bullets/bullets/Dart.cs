using System.Collections;
using UnityEngine;

public class Dart : MonoBehaviour
{
    private float poisonDuration = 5f;
    private float poisonInterval = 1f;

    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Color colorGreen = Color.green; // Set the color for poison damage
            CharacterStatManager enemy = collision.gameObject.GetComponentInParent<CharacterStatManager>();
            enemy.TakeDamage(PlayerUpgradeManager.Instance.blowDartStats.damage);

            StartCoroutine(ApplyPoisonDamage(PlayerUpgradeManager.Instance.blowDartStats.poisonDamage, enemy, colorGreen));

            spriteRenderer.enabled = false; // Hide the dart sprite
            boxCollider.enabled = false; // Disable the collider
        }
    }

    private IEnumerator ApplyPoisonDamage(float damage, CharacterStatManager enemy, Color color)
    {
        if (poisonDuration <= 0 && enemy.gameObject.activeInHierarchy)
        {

            Destroy(gameObject);
        }
        else
        {
            while (poisonDuration > 0 && enemy.gameObject.activeInHierarchy)
            {
                yield return new WaitForSeconds(poisonInterval);
                poisonDuration -= poisonInterval;

                // Apply poison damage to the enemy
                if(enemy.gameObject.activeInHierarchy && enemy != null)
                {
                    enemy.TakeDamage(damage, color);
                }
            }
        }
    }
}
