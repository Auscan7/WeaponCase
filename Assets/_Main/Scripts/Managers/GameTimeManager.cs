using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameTimeManager : MonoBehaviour
{
    public GameObject endBossPrefab;
    public Transform endBossSpawnPoint;

    private GameObject spawnedEndBoss;
    private bool bossSpawned = false;
    private bool bossDefeated = false;
    private bool spawnersPausedEarly = false;

    public static GameTimeManager Instance;

    public LoadingScreen loadingScreen; // Reference to the LoadingScreen script
    public string sceneToLoad = "Main Menu"; // Set this to whatever scene you want to load

    public TextMeshProUGUI timerText;
    public float triggerTime = 10f;

    private float remainingTime;
    public bool timerRunning = true;
    public GameObject WinScreen;
    public GameObject LoseScreen;

    public CanvasGroup winScreenCanvasGroup;
    private float winScreenFadeDuration = 1.25f;

    public CanvasGroup loseScreenCanvasGroup;
    private float loseScreenFadeDuration = 1.25f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        remainingTime = triggerTime;
        UpdateTimerText();
    }

    void Update()
    {
        if (timerRunning)
        {
            remainingTime -= Time.deltaTime;
            remainingTime = Mathf.Max(remainingTime, 0);
            UpdateTimerText();

            // Stop enemy spawns 20 seconds before boss appears
            if (remainingTime <= 30f && !spawnersPausedEarly)
            {
                spawnersPausedEarly = true;
                PauseAllWaveSpawners();
            }

            if (remainingTime <= 0f && !bossSpawned)
            {
                SpawnEndBoss();
            }
        }
    }


    private void SpawnEndBoss()
    {
        timerRunning = false;
        bossSpawned = true;

        var spawners = Object.FindObjectsByType<WaveSpawner>(FindObjectsSortMode.None);
        foreach (var spawner in spawners)
        {
            spawner.isPaused = true;
        }

        spawnedEndBoss = Instantiate(endBossPrefab, endBossSpawnPoint.position, Quaternion.identity);

        AudioManager.instance.PlayBossMusicWithFade();

        CameraShakeManager.Instance.Shake(3.5f, 0.1f);
    }

    public void OnBossDefeated()
    {
        bossDefeated = true;
        LevelCompletedTrigger();
    }


    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void LevelCompletedTrigger()
    {
        DifficultyManager.instance.UnlockNextDifficulty();

        timerRunning = false;
        PauseManager.instance.PauseGame();

        // Start fade in coroutine
        StartCoroutine(FadeInWinScreen());
    }

    private IEnumerator FadeInWinScreen()
    {
        WinScreen.SetActive(true);

        if (winScreenCanvasGroup != null)
        {
            winScreenCanvasGroup.alpha = 0f;

            float elapsed = 0f;

            while (elapsed < winScreenFadeDuration)
            {
                elapsed += Time.unscaledDeltaTime;  // Use unscaled for pause-safe fade
                winScreenCanvasGroup.alpha = Mathf.Clamp01(elapsed / winScreenFadeDuration);
                yield return null;
            }

            winScreenCanvasGroup.alpha = 1f;
        }
        else
        {
            Debug.LogWarning("winScreenCanvasGroup is not assigned!");
        }
    }

    private IEnumerator FadeInLoseScreen()
    {
        LoseScreen.SetActive(true);

        if (loseScreenCanvasGroup != null)
        {
            loseScreenCanvasGroup.alpha = 0f;

            float elapsed = 0f;

            while (elapsed < loseScreenFadeDuration)
            {
                elapsed += Time.unscaledDeltaTime;  // Use unscaled for pause-safe fade
                loseScreenCanvasGroup.alpha = Mathf.Clamp01(elapsed / loseScreenFadeDuration);
                yield return null;
            }

            loseScreenCanvasGroup.alpha = 1f;
        }
        else
        {
            Debug.LogWarning("loseScreenCanvasGroup is not assigned!");
        }
    }

    public void LevelFailedTrigger()
    {
        timerRunning = false;
        PauseManager.instance.PauseGame();

        // Start fade in coroutine
        StartCoroutine(FadeInLoseScreen());
    }
    private void PauseAllWaveSpawners()
    {
        var spawners = Object.FindObjectsByType<WaveSpawner>(FindObjectsSortMode.None);
        foreach (var spawner in spawners)
        {
            spawner.isPaused = true;
        }
    }

    public void RestartLevel()
    {
        WinScreen.SetActive(false);
        LoseScreen.SetActive(false);
        PauseManager.instance.UnPauseGame();
        loadingScreen.loadingScreen.SetActive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        WinScreen.SetActive(false);
        LoseScreen.SetActive(false);
        PauseManager.instance.UnPauseGame();
        AudioManager.instance.PlaySoundSFX(AudioManager.instance.UIClickSFX);
        loadingScreen.loadingScreen.SetActive(true);
        SceneManager.LoadScene(sceneToLoad);
    }
}