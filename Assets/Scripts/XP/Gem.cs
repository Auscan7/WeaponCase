using UnityEngine;

public class Gem : MonoBehaviour
{
    public int xpValue = 1; // Amount of XP this gem gives
    public float moveSpeed = 5f; // Speed at which the gem moves toward the player

    private Transform playerTransform; // Reference to the player's position
    private bool isMovingToPlayer = false; // Flag to determine if the gem should move toward the player

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
            // Start moving toward the player
            isMovingToPlayer = true;
        }
    }

    private void Update()
    {
        if (isMovingToPlayer && playerTransform != null)
        {
            // Move the gem toward the player's position
            transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);

            // Check if the gem has reached the player's position
            if (Vector2.Distance(transform.position, playerTransform.position) < 0.1f)
            {
                PlayerLevelSystem.instance.AddXP(xpValue); // Add XP
                Destroy(gameObject); // Destroy the gem
            }
        }
    }
}
