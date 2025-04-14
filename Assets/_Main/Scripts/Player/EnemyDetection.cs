using UnityEngine;
using System.Collections.Generic;

public class EnemyDetection : MonoBehaviour
{
    private List<Weapon> equippedWeapons = new List<Weapon>();

    [SerializeField] private LayerMask enemyLayers; // Allow multiple layers to be set in the Inspector

    public void Start()
    {
        // Find all equipped weapons automatically
        equippedWeapons.AddRange(GetComponents<Weapon>());
    }

    void Update()
    {
        DetectAndFireAtEnemy();
    }

    private void DetectAndFireAtEnemy()
    {
        foreach (Weapon weapon in equippedWeapons)
        {
            if (!weapon.gameObject.activeInHierarchy) continue; // Only process active weapons
            if (weapon.isManualWeapon) continue; // Skip manual weapons

            Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(
                transform.position,
                weapon.weaponRange,
                enemyLayers // Use multiple layers instead of just "Enemy"
            );

            if (enemiesInRange.Length > 0)
            {
                Collider2D closestEnemy = GetClosestEnemy(enemiesInRange);
                if (closestEnemy != null)
                {
                    weapon.RotateTowardsTarget(closestEnemy.transform.position);
                    weapon.Fire(closestEnemy.transform.position);
                }
            }
        }
    }

    private Collider2D GetClosestEnemy(Collider2D[] enemies)
    {
        Collider2D closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider2D enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = enemy;
            }
        }
        return closest;
    }
}
