using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternFishEnemyMovement : BasicEnemyMovement
{
    public override IEnumerator ProjectileAttackRoutine()
    {
        isAttacking = true;

        // Fire the projectile
        if (projectilePrefab != null && firePoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            Vector2 direction = (player.position - transform.position).normalized;

            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = direction * moveSpeed; // Set the speed of the projectile
            }
        }

        // Wait for the cooldown
        yield return new WaitForSeconds(fireCooldown);

        isAttacking = false;
        isOnCooldown = false;
    }
}
