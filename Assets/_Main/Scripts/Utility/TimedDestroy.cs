using UnityEngine;

public class TimedDestroy : MonoBehaviour
{
    [Header("Destruction Settings")]
    public float lifetime = 5f; // Time in seconds before the GameObject is destroyed

    void Awake()
    {
        // Schedule the destruction of this GameObject
        Destroy(gameObject, lifetime);
    }
}
