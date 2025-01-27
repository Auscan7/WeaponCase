using UnityEngine;
using System.Collections;

public class BasicEnemyMovement : MonoBehaviour
{
    public Animator animator;

    public float moveSpeed = 5f;
    public float coneAttackDamageAmount = 5f;
    public Transform player;
    public Transform spriteTransform;

    [Header("Cone Attack")]
    public bool hasConeAttack = false; // Toggle for cone attack
    public float attackDistance = 3f; // Distance to stop and attack
    public float indicatorDuration = 1f; // Time before attack happens
    public float coneAngle = 45f; // Angle of the cone
    public float coneRange = 5f; // Range of the cone attack
    public LayerMask playerLayer; // Layer to check for the player
    public GameObject attackIndicator; // Manually placed child GameObject for the attack indicator

    [Header("Projectile Attack")]
    public bool hasProjectileAttack = false; // Toggle for projectile attack
    public GameObject projectilePrefab; // Prefab for the projectile
    public Transform firePoint; // Transform from where the projectile is fired
    public float projectileRange = 7f; // Range for firing projectiles
    public float fireCooldown = 2f; // Cooldown between projectile attacks

    public float rotationSpeed = 5f; // Speed of rotation smoothing
    public float cooldownTime = 2f; // Cooldown time for the attack

    public bool isAttacking = false;
    public bool isOnCooldown = false; // Track if the attack is on cooldown
    public Vector3 savedDirection; // Save the direction to lock rotation during attack

    protected virtual void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        // Ensure the indicator is initially turned off
        if (attackIndicator != null)
        {
            attackIndicator.SetActive(false);
        }
    }

    protected virtual void Update()
    {
        if (player == null || spriteTransform == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (hasConeAttack && !isOnCooldown && distanceToPlayer <= attackDistance)
        {
            if (!isAttacking)
            {
                savedDirection = (player.position - transform.position).normalized; // Save current direction
                StartCoroutine(ConeAttackRoutine());
            }
        }
        else if (hasProjectileAttack && !isOnCooldown && distanceToPlayer <= projectileRange)
        {
            if (!isAttacking)
            {
                StartCoroutine(ProjectileAttackRoutine());
            }
        }
        else if (!isAttacking)
        {
            // Normal behavior: Move and rotate towards the player
            Vector2 direction = (player.position - transform.position).normalized;

            transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;

            if (direction != Vector2.zero)
            {
                float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                float smoothedAngle = Mathf.LerpAngle(spriteTransform.eulerAngles.z, targetAngle, rotationSpeed * Time.deltaTime);
                spriteTransform.rotation = Quaternion.Euler(new Vector3(0, 0, smoothedAngle));
            }
        }
    }

    protected virtual IEnumerator ConeAttackRoutine()
    {
        isAttacking = true;

        // Show the attack indicator
        if (attackIndicator != null)
        {
            attackIndicator.SetActive(true);
        }

        // Wait for the indicator duration
        yield return new WaitForSeconds(indicatorDuration);

        // Perform the attack
        if (attackIndicator != null)
        {
            attackIndicator.SetActive(false);
        }

        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position, coneRange, playerLayer);
        foreach (Collider2D hit in hitPlayers)
        {
            Vector2 toTarget = hit.transform.position - transform.position;
            float angle = Vector2.Angle(savedDirection, toTarget);
            if (angle <= coneAngle / 2)
            {
                hit.gameObject.GetComponentInParent<CharacterStatManager>().TakeDamage(coneAttackDamageAmount);
            }
        }

        yield return new WaitForSeconds(0.5f); // Short delay before starting cooldown

        // Start cooldown
        StartCoroutine(AttackCooldownRoutine());

        isAttacking = false;

        // Smoothly rotate back toward the player after attack ends
        StartCoroutine(SmoothTurnTowardsPlayer());
    }

    public virtual IEnumerator ProjectileAttackRoutine()
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

    protected virtual IEnumerator AttackCooldownRoutine()
    {
        isOnCooldown = true;

        // Wait for the cooldown time
        yield return new WaitForSeconds(cooldownTime);

        isOnCooldown = false;
    }

    protected virtual IEnumerator SmoothTurnTowardsPlayer()
    {
        while (isAttacking == false)
        {
            if (player == null || spriteTransform == null) yield break;

            // Calculate direction towards the player
            Vector2 direction = (player.position - transform.position).normalized;
            if (direction == Vector2.zero) yield break;

            // Smoothly interpolate rotation
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            float smoothedAngle = Mathf.LerpAngle(spriteTransform.eulerAngles.z, targetAngle, rotationSpeed * Time.deltaTime);
            spriteTransform.rotation = Quaternion.Euler(0, 0, smoothedAngle);

            // Exit the loop once the rotation is close to the target
            if (Mathf.Abs(Mathf.DeltaAngle(spriteTransform.eulerAngles.z, targetAngle)) < 0.1f)
            {
                spriteTransform.rotation = Quaternion.Euler(0, 0, targetAngle); // Snap to final rotation
                yield break;
            }

            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        if (hasConeAttack)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, coneRange);

            Vector3 rightLimit = Quaternion.Euler(0, 0, coneAngle / 2) * Vector3.right * coneRange;
            Vector3 leftLimit = Quaternion.Euler(0, 0, -coneAngle / 2) * Vector3.right * coneRange;

            Gizmos.DrawLine(transform.position, transform.position + rightLimit);
            Gizmos.DrawLine(transform.position, transform.position + leftLimit);
        }

        if (hasProjectileAttack)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, projectileRange);
        }
    }
}
