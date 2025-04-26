using UnityEngine;
using System.Collections;

public class HammerheadCombat : MonoBehaviour
{
    [SerializeField] private float coneAttackDamage;
    [SerializeField] private float coneAngle = 45f;
    [SerializeField] private float coneRange = 5f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private GameObject attackIndicator;

    private EnemyMovementManager movementManager;
    private Animator animator;
    private bool isAttacking = false;
    private Vector3 savedDirection;
    private Transform player;

    private void Awake()
    {
        movementManager = GetComponent<EnemyMovementManager>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        attackIndicator?.SetActive(false);
        savedDirection = transform.right; // Default direction
    }

    public void TryConeAttack(Transform player)
    {
        if (isAttacking) return; // Prevents multiple attack calls

        this.player = player; // Store player reference

        // Check if the player is within attack range
        if (Vector2.Distance(transform.position, player.position) > coneRange) return;

        savedDirection = (player.position - transform.position).normalized;
        StartCoroutine(ConeAttackRoutine());
    }

    private IEnumerator ConeAttackRoutine()
    {
        isAttacking = true;
        if (movementManager != null) movementManager.isAttacking = true; // Stop movement/rotation

        animator.Play("SharkAttackOpenMouth");

        if (attackIndicator) attackIndicator.SetActive(true);
        yield return new WaitForSeconds(1f); // Attack windup time

        if (attackIndicator) attackIndicator.SetActive(false);
        AudioManager.instance.PlaySoundSFX(AudioManager.instance.hammerheadSFX);
        if (player == null) // In case player moved/died
        {
            ResetAttack();
            yield break;
        }

        // Attack logic
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position, coneRange, playerLayer);
        foreach (Collider2D hit in hitPlayers)
        {
            Vector2 toTarget = hit.transform.position - transform.position;
            float angle = Vector2.Angle(savedDirection, toTarget);
            if (angle <= coneAngle / 2)
            {
                hit.GetComponentInParent<CharacterStatManager>()?.TakeDamage(coneAttackDamage);
            }
        }

        // **Re-enable movement after attack happens!**
        if (movementManager != null) movementManager.isAttacking = false;

        yield return new WaitForSeconds(attackCooldown); // Just waiting for cooldown, movement should still happen

        isAttacking = false; // Allow a new attack
    }


    private void ResetAttack()
    {
        isAttacking = false;
        if (movementManager != null) movementManager.isAttacking = false; // Resume movement
    }

    private void OnDrawGizmos()
    {
        if (savedDirection == Vector3.zero) savedDirection = transform.right; // Ensure it's valid

        Gizmos.color = Color.red;
        Vector3 leftBoundary = Quaternion.Euler(0, 0, -coneAngle / 2) * savedDirection * coneRange;
        Vector3 rightBoundary = Quaternion.Euler(0, 0, coneAngle / 2) * savedDirection * coneRange;

        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
        Gizmos.DrawWireSphere(transform.position, coneRange);
    }
}
