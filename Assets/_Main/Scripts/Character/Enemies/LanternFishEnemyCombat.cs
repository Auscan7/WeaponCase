using UnityEngine;
using System.Collections;

public class LanternFishCombat : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float fireCooldown = 2f;
    private bool isOnCooldown = false;

    public void TryProjectileAttack(Transform player)
    {
        if (isOnCooldown) return;
        StartCoroutine(ProjectileAttackRoutine(player));
    }

    private IEnumerator ProjectileAttackRoutine(Transform player)
    {
        isOnCooldown = true;
        if (projectilePrefab != null && firePoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = (player.position - firePoint.position).normalized * 2.5f;
            }
        }

        yield return new WaitForSeconds(fireCooldown);
        isOnCooldown = false;
    }
}
