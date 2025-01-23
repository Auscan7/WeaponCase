using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletDamage;

    private void Awake()
    {
        bulletDamage = WeaponManager.Instance.pistolDamage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponentInParent<CharacterStatManager>().TakeDamage(bulletDamage);

            Destroy(gameObject);
        }
    }
}
