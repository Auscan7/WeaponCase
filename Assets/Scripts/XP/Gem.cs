using UnityEngine;
using static GemPoolManager;

public class Gem : MonoBehaviour
{
    public GemType gemType; // Set this in the inspector per prefab
    public int xpValue = 1; // Amount of XP this gem gives
    public float moveSpeed = 5f; // Speed at which the gem moves toward the player

    private Transform playerTransform;
    private bool isMovingToPlayer = false;

    private void Start()
    {
        // Find the player's transform. Adjust the tag if needed.
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
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

            if (Vector2.Distance(transform.position, playerTransform.position) < 0.1f)
            {
                AudioManager.instance.PlaySoundSFX(AudioManager.instance.gemPickUp);
                PlayerLevelSystem.instance.AddXP(xpValue);

                // Return gem to the pool instead of destroying it
                GemPoolManager.Instance.ReturnGem(gemType, gameObject);
            }
        }
    }
}