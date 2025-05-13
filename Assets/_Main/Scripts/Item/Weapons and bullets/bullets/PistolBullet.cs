using UnityEngine;

public class PistolBullet : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponentInParent<CharacterStatManager>().TakeDamage(PlayerUpgradeManager.Instance.pistolStats.damage);

            EffectsManager.instance.PlayVFX(EffectsManager.instance.bulletHitVFX, collision.transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
