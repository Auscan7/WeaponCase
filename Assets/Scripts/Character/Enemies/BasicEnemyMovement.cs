using UnityEngine;
using System.Collections;

public class BasicEnemyMovement : CharacterMovement
{
    public float moveSpeed = 5f;
    public float damageAmount = 5f;
    private Transform player;
    public Transform spriteTransform;
    public bool hasConeAttack = false; // Toggle for cone attack
    public float attackDistance = 3f; // Distance to stop and attack
    public float indicatorDuration = 1f; // Time before attack happens
    public float coneAngle = 45f; // Angle of the cone
    public float coneRange = 5f; // Range of the cone attack
    public LayerMask playerLayer; // Layer to check for the player
    public GameObject attackIndicator; // Manually placed child GameObject for the attack indicator
    public float rotationSpeed = 5f; // Speed of rotation smoothing
    public float cooldownTime = 2f; // Cooldown time for the attack

    private bool isAttacking = false;
    private bool isOnCooldown = false; // Track if the attack is on cooldown
    private Vector3 savedDirection; // Save the direction to lock rotation during attack

    protected override void Start()
    {
        base.Start();

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

    protected override void Update()
    {
        base.Update();

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

    private IEnumerator ConeAttackRoutine()
    {
        isAttacking = true;

        // Show the attack indicator
        if (attackIndicator != null)
        {
            attackIndicator.SetActive(true);
            //attackIndicator.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(savedDirection.y, savedDirection.x) * Mathf.Rad2Deg);
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
                Debug.Log("Player hit by cone attack!");
                hit.gameObject.GetComponentInParent<CharacterStatManager>().TakeDamage(damageAmount);
            }
        }

        yield return new WaitForSeconds(0.5f); // Short delay before starting cooldown

        // Start cooldown
        StartCoroutine(AttackCooldownRoutine());

        isAttacking = false;

        // Smoothly rotate back toward the player after attack ends
        StartCoroutine(SmoothTurnTowardsPlayer());
    }

    private IEnumerator AttackCooldownRoutine()
    {
        isOnCooldown = true;

        // Wait for the cooldown time
        yield return new WaitForSeconds(cooldownTime);

        isOnCooldown = false;
    }

    private IEnumerator SmoothTurnTowardsPlayer()
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
            // Draw the cone area for debugging
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, coneRange);

            Vector3 rightLimit = Quaternion.Euler(0, 0, coneAngle / 2) * Vector3.right * coneRange;
            Vector3 leftLimit = Quaternion.Euler(0, 0, -coneAngle / 2) * Vector3.right * coneRange;

            Gizmos.DrawLine(transform.position, transform.position + rightLimit);
            Gizmos.DrawLine(transform.position, transform.position + leftLimit);
        }
    }
}
