using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyViewportManager : MonoBehaviour
{
    private Camera mainCamera;
    private Rigidbody2D rb;
    private bool wasVisible = false;

    private void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!mainCamera) return;

        // Convert the enemy's world position to viewport position
        Vector3 viewportPoint = mainCamera.WorldToViewportPoint(transform.position);

        // Define an offset to extend the viewport boundaries
        float offset = 2f;

        // Check if the enemy is visible in the viewport with the offset
        bool isVisible = viewportPoint.x >= -offset && viewportPoint.x <= 1 + offset &&
                        viewportPoint.y >= -offset && viewportPoint.y <= 1 + offset &&
                        viewportPoint.z > 0;

        // Add a small buffer zone to prevent rapid enabling/disabling at the edges
        if (!wasVisible && isVisible)
        {
            // Enemy just entered the viewport
            EnablePhysics();
        }
        else if (wasVisible && !isVisible)
        {
            // Enemy just left the viewport
            DisablePhysics();
        }

        wasVisible = isVisible;
    }

    private void EnablePhysics()
    {
        rb.simulated = true;
    }

    private void DisablePhysics()
    {
        rb.simulated = false;
    }
} 