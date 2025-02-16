using UnityEngine;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;

public class FloatingTextManager : MonoBehaviour
{
    public static FloatingTextManager Instance { get; private set; }

    [SerializeField] private GameObject floatingTextPrefab;
    private int poolSize = 100; // Adjust based on your needs

    private Queue<GameObject> floatingTextPool = new Queue<GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializePool();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializePool()
    {
        // Pre-instantiate a pool of floating texts
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(floatingTextPrefab);
            obj.SetActive(false);
            floatingTextPool.Enqueue(obj);
        }
    }

    public void ShowFloatingText(Vector3 position, string text, Color color, float scale, float scaleDuration, float fadeOutDuration, float delay = 0.8f)
    {
        GameObject floatingText = GetPooledObject();
        if (floatingText == null) return; // Should rarely happen

        // Position the text with a random offset for variety
        floatingText.transform.position = position + GetRandomOffset();
        floatingText.transform.rotation = Quaternion.identity;
        floatingText.SetActive(true);

        TMP_Text textComponent = floatingText.GetComponentInChildren<TMP_Text>();
        if (textComponent != null)
        {
            // Cancel any previous animations on this object
            textComponent.DOKill();

            // Reset alpha in case it was faded out before
            Color newColor = color;
            newColor.a = 1f;
            textComponent.color = newColor;

            textComponent.text = text;
            textComponent.transform.localScale = Vector3.zero;

            if (floatingText != null)
            {
                // Scale-up animation
                textComponent.transform.DOScale(scale, scaleDuration).SetEase(Ease.OutBack);
            }

            if (floatingText != null)
            {
                // Fade out animation then return to pool
                textComponent.DOFade(0, fadeOutDuration)
                             .SetDelay(delay)
                             .OnComplete(() => ReturnToPool(floatingText));
            }
        }
    }

    private Vector3 GetRandomOffset()
    {
        // Customize these ranges as needed
        return new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(0.5f, 1f), 0);
    }

    private GameObject GetPooledObject()
    {
        if (floatingTextPool.Count > 0)
        {
            return floatingTextPool.Dequeue();
        }
        else
        {
            Debug.LogWarning("Pool is empty! Consider increasing the pool size.");
            // Optionally, instantiate a new object if the pool is empty.
            return Instantiate(floatingTextPrefab);
        }
    }

    private void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        floatingTextPool.Enqueue(obj);
    }
}
