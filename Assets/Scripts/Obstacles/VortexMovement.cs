using UnityEngine;

public class VortexMovement : MonoBehaviour
{
    private Vector3 moveDirection;
    private float speed;

    public void Initialize(Vector3 direction, float moveSpeed)
    {
        moveDirection = direction;
        speed = moveSpeed;
    }

    private void Update()
    {
        // Move the vortex in the calculated direction
        transform.position += moveDirection * speed * Time.deltaTime;

        // Optional: Destroy the vortex after it has traveled far enough
        if (Vector3.Distance(transform.position, moveDirection) > 100f) // Adjust the range as needed
        {
            Destroy(gameObject);
        }
    }
}
