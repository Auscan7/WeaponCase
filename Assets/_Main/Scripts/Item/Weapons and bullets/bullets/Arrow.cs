using UnityEngine;
using System.Collections.Generic;

public class Arrow : MonoBehaviour
{
    public int maxHops = 3; // adjustable hop count
    private float hopRange = 7.5f; // range to search for next target
    private int currentHopCount = 0;
    private Transform currentTarget;
    private float speed;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        speed = rb.linearVelocity.magnitude; // maintain initial speed
    }

    private void Update()
    {
        if (currentTarget != null)
        {
            Vector2 direction = ((Vector2)currentTarget.position - rb.position).normalized;
            rb.linearVelocity = direction * speed;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // Damage logic
            collision.GetComponentInParent<CharacterStatManager>()?.TakeDamage(PlayerUpgradeManager.Instance.bowAndArrowStats.damage);

            currentHopCount++;

            if (currentHopCount >= maxHops)
            {
                Destroy(gameObject);
                return;
            }

            // Find next target
            Transform nextTarget = FindNextTarget(collision.transform);

            if (nextTarget != null)
            {
                currentTarget = nextTarget;
            }
            else
            {
                Destroy(gameObject); // no valid targets left
            }
        }
    }

    private Transform FindNextTarget(Transform currentHit)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        Transform closest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            if (enemy.transform == currentHit) continue;

            float dist = Vector2.Distance(transform.position, enemy.transform.position);
            if (dist < minDist && dist <= hopRange)
            {
                minDist = dist;
                closest = enemy.transform;
            }
        }

        return closest;
    }

    // Optional setup from BowAndArrow script
    public void Initialize(Vector2 initialVelocity, int hops, float range)
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = initialVelocity;
        speed = initialVelocity.magnitude;
        maxHops = hops;
        hopRange = range;
    }
}
