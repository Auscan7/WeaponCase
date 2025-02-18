using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    private Transform player; // Reference to the player's transform
    [SerializeField] private float followSpeed = 5f; // Speed of following
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10); // Default offset to keep the camera at the correct Z position

    public static CameraFollowPlayer instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        // Find the player by tag
        player = GameObject.FindWithTag("Player")?.transform;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (player == null)
        {
            // Find the player by tag
            player = GameObject.FindWithTag("Player")?.transform;
        }

        if (player != null)
        {
            // Smoothly interpolate the camera's position towards the player's position + offset
            Vector3 targetPosition = player.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
    }
}
