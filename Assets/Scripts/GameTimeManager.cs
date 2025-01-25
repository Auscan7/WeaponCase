using UnityEngine;
using TMPro;

public class GameTimeManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float triggerTime = 10f;

    private float remainingTime;
    private bool timerRunning = true;

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
                TriggerCustomLogic();
            }
        }
    }

    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void TriggerCustomLogic()
    {
        timerRunning = false;
        Debug.Log("Custom logic triggered! Countdown reached 0.");
        // Add your custom logic here
    }
}