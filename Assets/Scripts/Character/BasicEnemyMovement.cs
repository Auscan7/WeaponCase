using UnityEngine;

public class BasicEnemyMovement : CharacterMovement
{
    public float moveSpeed = 5f;
    private Transform player; // Reference to the player
    public Transform spriteTransform; // Reference to the child object containing the sprite

    protected override void Start()
    {
        base.Start();

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    protected override void Update()
    {
        base.Update();

        if (player == null || spriteTransform == null) return;

        // Calculate direction towards the player
        Vector2 direction = (player.position - transform.position).normalized;

        // Update position
        transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;

        // Rotate the sprite to face the player
        if (direction != Vector2.zero) // Ensure we have a valid direction
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            spriteTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 0)); // Adjust to match sprite orientation
        }
    }
}
