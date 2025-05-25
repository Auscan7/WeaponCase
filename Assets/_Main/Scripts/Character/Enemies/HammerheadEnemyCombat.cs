using UnityEngine;
using System.Collections;

public class HammerheadCombat : MonoBehaviour
{
    [Header("Combat Settings")]
    [SerializeField] private float coneAttackDamage;
    [SerializeField] private float coneAngle = 45f;
    [SerializeField] private float coneRange = 5f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private GameObject attackIndicator;

    [Header("Sprite Swap")]
    [SerializeField] private SpriteRenderer spriteRenderer; // Reference to the sprite
    [SerializeField] private Sprite closedMouthSprite;     // Normal idle sprite
    [SerializeField] private Sprite openMouthSprite;       // Mouth open attack sprite

    private EnemyMovementManager movementManager;
    private bool isAttacking = false;
    private Vector3 savedDirection;
    private Transform player;

    private void Awake()
    {
        movementManager = GetComponent<EnemyMovementManager>();
    }

    private void Start()
    {
        float multiplier = DifficultyManager.instance.GetCurrentEnemyDamageMultiplier();
        coneAttackDamage *= multiplier;

        attackIndicator?.SetActive(false);
        savedDirection = transform.right;
    }

    public void TryConeAttack(Transform player)
    {
        if (isAttacking) return;

        this.player = player;
        if (Vector2.Distance(transform.position, player.position) > coneRange) return;

        savedDirection = (player.position - transform.position).normalized;
        StartCoroutine(ConeAttackRoutine());
    }

    private IEnumerator ConeAttackRoutine()
    {
        isAttacking = true;
        if (movementManager != null) movementManager.isAttacking = true;

        // Swap to open mouth sprite
        if (spriteRenderer && openMouthSprite) spriteRenderer.sprite = openMouthSprite;

        if (attackIndicator) attackIndicator.SetActive(true);
        yield return new WaitForSeconds(1f); // Windup

        if (attackIndicator) attackIndicator.SetActive(false);
        AudioManager.instance.PlaySoundSFX(AudioManager.instance.hammerheadSFX);

        if (player == null)
        {
            ResetAttack();
            yield break;
        }

        // Deal damage in cone
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

        // Swap back to idle sprite
        if (spriteRenderer && closedMouthSprite) spriteRenderer.sprite = closedMouthSprite;

        if (movementManager != null) movementManager.isAttacking = false;

        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    private void ResetAttack()
    {
        isAttacking = false;
        if (spriteRenderer && closedMouthSprite) spriteRenderer.sprite = closedMouthSprite;
        if (movementManager != null) movementManager.isAttacking = false;
    }

    private void OnDrawGizmos()
    {
        if (savedDirection == Vector3.zero) savedDirection = transform.right;

        Gizmos.color = Color.red;
        Vector3 leftBoundary = Quaternion.Euler(0, 0, -coneAngle / 2) * savedDirection * coneRange;
        Vector3 rightBoundary = Quaternion.Euler(0, 0, coneAngle / 2) * savedDirection * coneRange;

        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
        Gizmos.DrawWireSphere(transform.position, coneRange);
    }
}
