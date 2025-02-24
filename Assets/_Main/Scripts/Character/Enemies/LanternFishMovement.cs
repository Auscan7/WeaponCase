using UnityEngine;

public class LanternFishMovement : EnemyMovementManager
{
    private float stopDistance = 7.5f;
    private float kiteDistance = 5.5f;

    protected override void Update()
    {
        if (player == null || isAttacking) return; // Stop moving/rotating when attacking

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        Vector2 direction = Vector2.zero; // Default: No movement

        if (distanceToPlayer > stopDistance)
        {
            // Move toward the player if too far
            direction = (player.position - transform.position).normalized;
        }
        else if (distanceToPlayer <= stopDistance && distanceToPlayer >= kiteDistance)
        {
            // Stop moving if within the desired range
            direction = Vector2.zero;
        }
        else if (distanceToPlayer < kiteDistance)
        {
            // Move away from the player if too close (kiting)
            direction = (transform.position - player.position).normalized;
        }

        // Apply movement if direction isn't zero
        Move(direction, moveSpeed);

        // 🔹 Rotate towards movement direction instead of always facing the player
        if (direction != Vector2.zero)
        {
            RotateTowards(direction);
        }
    }
}
