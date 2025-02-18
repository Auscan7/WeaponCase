using UnityEngine;
using static GemPoolManager;

public class Gem : MonoBehaviour
{
    public GemType gemType; // Set this in the inspector per prefab
    public int xpValue = 1; // XP this gem gives
    public float moveSpeed = 5f; // Speed at which the gem moves toward the player
    public float collectDistance = 0.5f; // Distance threshold for collection

    private Transform playerTransform;
    private bool isMovingToPlayer = false;
    private Collider2D gemCollider;

    private void Awake()
    {
        gemCollider = GetComponent<Collider2D>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    private void OnEnable()
    {
        isMovingToPlayer = false;
        if (gemCollider != null)
        {
            gemCollider.enabled = false;
            Invoke(nameof(EnableCollider), 0.2f);
        }
    }

    private void EnableCollider()
    {
        if (gemCollider != null)
            gemCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Collector"))
        {
            isMovingToPlayer = true;
        }
    }

    private void Update()
    {
        if (isMovingToPlayer && playerTransform != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, playerTransform.position) < collectDistance)
            {
                AudioManager.instance.PlaySoundSFX(AudioManager.instance.gemPickUp);
                PlayerLevelSystem.instance.AddXP(xpValue);

                // Return gem to the pool
                GemPoolManager.Instance.ReturnGem(gemType, gameObject);
            }
        }
    }
}
