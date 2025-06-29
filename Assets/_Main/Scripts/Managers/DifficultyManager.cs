using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager instance;

    public float[] enemyHealthMultipliers;
    public float[] enemyDamageMultipliers;
    public int currentDifficulty = 0;
    private const string UNLOCKED_KEY = "UnlockedDifficulty";

    public int unlockedDifficulty = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadDifficultyProgress();

            // Always reset to easiest difficulty at the start of each run
            currentDifficulty = 0;
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
            Debug.Log("Difficulty set to: " + level);
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
        if (currentDifficulty == unlockedDifficulty && unlockedDifficulty < enemyHealthMultipliers.Length - 1)
        {
            unlockedDifficulty++;
            PlayerPrefs.SetInt(UNLOCKED_KEY, unlockedDifficulty);
            PlayerPrefs.Save();
        }
    }

    private void LoadDifficultyProgress()
    {
        unlockedDifficulty = PlayerPrefs.GetInt(UNLOCKED_KEY, 0);
    }
}
