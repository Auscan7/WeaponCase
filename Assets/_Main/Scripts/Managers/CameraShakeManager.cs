using UnityEngine;
using System.Collections;

public class CameraShakeManager : MonoBehaviour
{
    public static CameraShakeManager Instance { get; private set; }
    public bool ShakeEnabled { get; set; } = true;


    private Transform camTransform;
    private Vector3 originalLocalPos;
    private Coroutine shakeCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        UpdateCameraReference();
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        UpdateCameraReference();
    }

    private void UpdateCameraReference()
    {
        if (Camera.main != null)
        {
            camTransform = Camera.main.transform;
            originalLocalPos = camTransform.localPosition;
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Shake(float duration, float magnitude)
    {
        if (!ShakeEnabled) return;

        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            camTransform.localPosition = originalLocalPos;
        }

        shakeCoroutine = StartCoroutine(ShakeCoroutine(duration, magnitude));
    }

    private IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            if (camTransform == null) yield break;

            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            camTransform.localPosition = new Vector3(originalLocalPos.x + x, originalLocalPos.y + y, originalLocalPos.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        if (camTransform != null)
            camTransform.localPosition = originalLocalPos;

        shakeCoroutine = null;
    }

}
