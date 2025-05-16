using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pellet : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponentInParent<CharacterStatManager>().TakeDamage(PlayerUpgradeManager.Instance.shotgunStats.damage);
            EffectsManager.instance.PlayVFX(EffectsManager.instance.bulletHitVFX, collision.transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
