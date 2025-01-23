using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pellet : MonoBehaviour
{
    [SerializeField] private float pelletDamage;

    private void Awake()
    {
        pelletDamage = WeaponManager.Instance.shotgunDamage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponentInParent<CharacterStatManager>().TakeDamage(pelletDamage);

            Destroy(gameObject);
        }
    }
}
