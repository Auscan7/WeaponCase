using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JuicyButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalScale;
    private Image buttonImage;
    private Color originalColor;

    [Header("Juice Settings")]
    public float punchScale = 1.1f;
    public float punchTime = 0.1f;
    public Color flashColor = Color.white;

    [Header("Hover Bounce Settings")]
    public bool enableHoverBounce = true;
    public float bounceAmount = 0.05f; // 5% up and down
    public float bounceSpeed = 2f;      // How fast it bounces

    private bool isHovering = false;
    private float bounceTimer = 0f;


    private void Awake()
    {
        originalScale = transform.localScale;
        buttonImage = GetComponent<Image>();
        if (buttonImage != null)
            originalColor = buttonImage.color;
    }


    private void Update()
    {
        if (enableHoverBounce && isHovering)
        {
            bounceTimer += Time.unscaledDeltaTime * bounceSpeed;
            float scaleOffset = Mathf.Sin(bounceTimer) * bounceAmount;
            transform.localScale = originalScale * (1f + scaleOffset);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(ScaleButton(originalScale * punchScale, punchTime));
        if (buttonImage != null)
            buttonImage.color = flashColor;

        isHovering = false;
        transform.localScale = originalScale;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(ScaleButton(originalScale, punchTime));
        if (buttonImage != null)
            buttonImage.color = originalColor;

        isHovering = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        bounceTimer = 0f; // reset the timer for smooth start
        AudioManager.instance.PlaySoundSFX(AudioManager.instance.UIHoverSFX); // Play hover sound
        if (buttonImage != null)
            buttonImage.color = Color.Lerp(originalColor, flashColor, 0.3f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        transform.localScale = originalScale; // Reset scale immediately
        if (buttonImage != null)
            buttonImage.color = originalColor;
    }

    private System.Collections.IEnumerator ScaleButton(Vector3 targetScale, float duration)
    {
        Vector3 startScale = transform.localScale;
        float time = 0f;
        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            float t = time / duration;
            t = Mathf.Sin(t * Mathf.PI * 0.5f); // EaseOutSine
            transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }
        transform.localScale = targetScale;
    }
}
