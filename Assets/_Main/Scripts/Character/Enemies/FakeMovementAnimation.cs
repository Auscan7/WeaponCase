using UnityEngine;

public class FakeMovementAnimation : MonoBehaviour
{
    [Header("Scale Animation Settings")]
    public float scaleSpeed = 2f;           // How fast it scales
    public float scaleAmount = 0.1f;        // How much it scales (0.1 = 10%)
    public bool scaleX = true;
    public bool scaleY = true;

    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        float scaleFactor = 1f + Mathf.Sin(Time.time * scaleSpeed) * scaleAmount;

        float x = scaleX ? originalScale.x * scaleFactor : originalScale.x;
        float y = scaleY ? originalScale.y * scaleFactor : originalScale.y;

        transform.localScale = new Vector3(x, y, originalScale.z);
    }
}