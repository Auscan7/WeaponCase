using UnityEngine;

public class SMGBullet : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponentInParent<CharacterStatManager>().TakeDamage(PlayerUpgradeManager.Instance.smgStats.damage);

            Destroy(gameObject);
        }
    }
}
