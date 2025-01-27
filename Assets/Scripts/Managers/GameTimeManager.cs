using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameTimeManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float triggerTime = 10f;

    private float remainingTime;
    private bool timerRunning = true;
    public GameObject WinScreen;

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
    }

    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void LevelCompletedTrigger()
    {
        timerRunning = false;
        PauseManager.instance.PauseGame();
        WinScreen.SetActive(true);
    }

    public void RestartLevel()
    {
        PauseManager.instance.UnPauseGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        PauseManager.instance.UnPauseGame();
        AudioManager.instance.PlaySoundSFX(AudioManager.instance.UIClickSFX);
        SceneManager.LoadScene("Main Menu");
    }
}