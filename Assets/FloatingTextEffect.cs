using System.Collections;
using UnityEngine;
using TMPro;

public class FloatingTextEffect : MonoBehaviour
{
    public float moveSpeed = 50f;           // Speed at which the text moves upwards (adjust for UI)
    public float fadeDuration = 2f;        // Duration for the text to fade out
    public Vector3 moveDirection = Vector3.up; // Direction of movement (upwards by default)

    private TextMeshProUGUI textMesh;
    private RectTransform rectTransform;  // Reference to the RectTransform
    private Color originalColor;

    void Awake()
    {
        // Get the TextMeshProUGUI and RectTransform components
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
        rectTransform = GetComponentInChildren<RectTransform>();

        if (textMesh == null || rectTransform == null)
        {
            Debug.LogError("TextMeshProUGUI or RectTransform component not found!");
            return;
        }

        // Store the original color
        originalColor = textMesh.color;
    }

    void Start()
    {
        // Start the floating and fading effect
        StartCoroutine(FloatingAndFading());
    }

    private IEnumerator FloatingAndFading()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            // Move the text upwards by modifying the RectTransform position
            rectTransform.anchoredPosition += (Vector2)(moveDirection * moveSpeed * Time.deltaTime);

            // Fade out the text
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            textMesh.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            // Increment elapsed time
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the text is fully transparent at the end
        textMesh.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        // Optionally destroy the object after fading out
        Destroy(gameObject);
    }
}
