using UnityEngine;
using System.Collections.Generic;

public class Arrow : MonoBehaviour
{
    public int maxHops = 3;
    private float hopRange = 10f;
    private int currentHopCount = 0;
    private Transform currentTarget;
    private float speed;
    private Rigidbody2D rb;

    // Keep track of already hit enemies
    private HashSet<Transform> hitEnemies = new HashSet<Transform>();

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        speed = rb.linearVelocity.magnitude;
    }

    private void Update()
    {
        if (currentTarget != null)
        {
            // Check if the target is still active
            if (!currentTarget.gameObject.activeInHierarchy)
            {
                TryFindNewTargetOrDestroy();
                return;
            }

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
            Transform hitTarget = collision.transform;

            // Avoid processing same enemy again
            if (hitEnemies.Contains(hitTarget)) return;

            // Damage logic
            collision.GetComponentInParent<CharacterStatManager>()?.TakeDamage(PlayerUpgradeManager.Instance.bowAndArrowStats.damage);

            hitEnemies.Add(hitTarget);
            currentHopCount++;

            if (currentHopCount >= maxHops)
            {
                Destroy(gameObject);
                return;
            }

            // Find next valid target
            Transform nextTarget = FindNextTarget();
            if (nextTarget != null)
            {
                currentTarget = nextTarget;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private void TryFindNewTargetOrDestroy()
    {
        Transform nextTarget = FindNextTarget();
        if (nextTarget != null)
        {
            currentTarget = nextTarget;
        }
        else
        {
            currentTarget = null; // Fly straight in current direction
        }
    }

    private Transform FindNextTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        Transform closest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            Transform enemyTransform = enemy.transform;

            if (hitEnemies.Contains(enemyTransform)) continue;

            float dist = Vector2.Distance(transform.position, enemyTransform.position);
            if (dist < minDist && dist <= hopRange)
            {
                minDist = dist;
                closest = enemyTransform;
            }
        }

        return closest;
    }

    public void Initialize(Vector2 initialVelocity, int hops, float range)
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = initialVelocity;
        speed = initialVelocity.magnitude;
        maxHops = hops;
        hopRange = range;
    }
}
