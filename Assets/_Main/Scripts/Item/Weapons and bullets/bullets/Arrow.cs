using UnityEngine;

public class Arrow : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponentInParent<CharacterStatManager>().TakeDamage(PlayerUpgradeManager.Instance.bowAndArrowStats.damage);

            Destroy(gameObject);
        }
    }
}
