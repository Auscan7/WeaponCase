using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager instance;

    public float[] enemyHealthMultipliers; // Array for each difficulty level
    public float[] enemyDamageMultipliers; // Array for each difficulty level
    public int currentDifficulty = 0; // Starts at 0 (Easy)
    private const string UNLOCKED_KEY = "UnlockedDifficulty";
    private const string SELECTED_KEY = "SelectedDifficulty";

    public int unlockedDifficulty = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Optional
            LoadDifficultyProgress();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetDifficulty(int level)
    {
        if (level <= unlockedDifficulty)
        {
            currentDifficulty = level;
            PlayerPrefs.SetInt(SELECTED_KEY, currentDifficulty);
        }
    }

    public float GetCurrentEnemyHealthMultiplier()
    {
        if (currentDifficulty >= 0 && currentDifficulty < enemyHealthMultipliers.Length)
            return enemyHealthMultipliers[currentDifficulty];
        return 1f;
    }

    public float GetCurrentEnemyDamageMultiplier()
    {
        if (currentDifficulty >= 0 && currentDifficulty < enemyDamageMultipliers.Length)
            return enemyDamageMultipliers[currentDifficulty];
        return 1f;
    }

    public void UnlockNextDifficulty()
    {
        if (unlockedDifficulty < enemyHealthMultipliers.Length - 1)
        {
            unlockedDifficulty++;
            PlayerPrefs.SetInt(UNLOCKED_KEY, unlockedDifficulty);
        }
    }

    private void LoadDifficultyProgress()
    {
        unlockedDifficulty = PlayerPrefs.GetInt(UNLOCKED_KEY, 0);
        currentDifficulty = PlayerPrefs.GetInt(SELECTED_KEY, 0);
    }
}
