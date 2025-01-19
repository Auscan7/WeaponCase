using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform player; // Reference to the player's transform
    [SerializeField] private float followSpeed = 5f; // Speed of following
    [SerializeField] private Vector3 offset = Vector3.zero; // Optional offset from the player

    void Update()
    {
        if (player != null)
        {
            // Smoothly interpolate the position of the object towards the player's position + offset
            Vector3 targetPosition = player.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
    }
}
