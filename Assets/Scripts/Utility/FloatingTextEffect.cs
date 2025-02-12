using System.Collections;
using UnityEngine;
using TMPro;

public class FloatingTextEffect : MonoBehaviour
{
    public float moveSpeed = 50f;           // Speed at which the text moves upwards (adjust for UI)
    public float fadeDuration = 2f;        // Duration for the text to fade out
    public Vector3 moveDirection = Vector3.up; // Direction of movement (upwards by default)

    private TextMeshProUGUI[] textMeshes;  // Array to hold multiple TextMeshProUGUI components
    private RectTransform rectTransform;   // Reference to the RectTransform
    private Color[] originalColors;        // Store original colors for each text

    void Start()
    {
        // Get all TextMeshProUGUI components in children
        textMeshes = GetComponentsInChildren<TextMeshProUGUI>();

        // Get the RectTransform of the parent object
        rectTransform = GetComponent<RectTransform>();

        if (textMeshes.Length == 0 || rectTransform == null)
        {
            Debug.LogError("No TextMeshProUGUI components found in children or RectTransform missing!");
            return;
        }

        // Store the original colors of each text element
        originalColors = new Color[textMeshes.Length];
        for (int i = 0; i < textMeshes.Length; i++)
        {
            originalColors[i] = textMeshes[i].color;
        }

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

            // Fade out each text component
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            for (int i = 0; i < textMeshes.Length; i++)
            {
                textMeshes[i].color = new Color(originalColors[i].r, originalColors[i].g, originalColors[i].b, alpha);
            }

            // Increment elapsed time
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure all texts are fully transparent at the end
        for (int i = 0; i < textMeshes.Length; i++)
        {
            textMeshes[i].color = new Color(originalColors[i].r, originalColors[i].g, originalColors[i].b, 0f);
        }

        // Optionally destroy the object after fading out
        Destroy(gameObject);
    }
}
