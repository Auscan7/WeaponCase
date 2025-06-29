using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameTimeManager : MonoBehaviour
{
    public static GameTimeManager Instance;

    public LoadingScreen loadingScreen; // Reference to the LoadingScreen script
    public string sceneToLoad = "Main Menu"; // Set this to whatever scene you want to load

    public TextMeshProUGUI timerText;
    public float triggerTime = 10f;

    private float remainingTime;
    public bool timerRunning = true;
    public GameObject WinScreen;
    public GameObject LoseScreen;

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

            // Clamp the timer to ensure it doesn't go below 0
            remainingTime = Mathf.Max(remainingTime, 0);

            UpdateTimerText();

            if (remainingTime <= 0)
            {
                LevelCompletedTrigger();
            }
        }

        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    LevelCompletedTrigger();
        //}
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
        WinScreen.SetActive(true);
    }

    public void LevelFailedTrigger()
    {
        timerRunning = false;
        PauseManager.instance.PauseGame();
        LoseScreen.SetActive(true);
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