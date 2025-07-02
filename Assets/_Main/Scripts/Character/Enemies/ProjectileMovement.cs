using System.Collections;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    private Vector2 direction;
    private float speed;
    private float damage;

    private Coroutine lifeCoroutine;

    public void SetDirection(Vector2 dir, float spd, float dmg)
    {
        direction = dir.normalized;
        speed = spd;
        damage = dmg;

        // Restart lifetime coroutine
        if (lifeCoroutine != null)
            StopCoroutine(lifeCoroutine);
        lifeCoroutine = StartCoroutine(LifetimeCoroutine());
    }

    private void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    private IEnumerator LifetimeCoroutine()
    {
        yield return new WaitForSeconds(20f);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponentInParent<CharacterStatManager>()?.TakeDamage(damage);
            gameObject.SetActive(false);

            if (lifeCoroutine != null)
            {
                StopCoroutine(lifeCoroutine);
                lifeCoroutine = null;
            }
        }
    }

    private void OnDisable()
    {
        if (lifeCoroutine != null)
        {
            StopCoroutine(lifeCoroutine);
            lifeCoroutine = null;
        }
    }
}
