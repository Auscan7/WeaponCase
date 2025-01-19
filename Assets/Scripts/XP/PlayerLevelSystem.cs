using UnityEngine;
using UnityEngine.UI;

public class PlayerLevelSystem : MonoBehaviour
{
    public static PlayerLevelSystem instance;

    [Header("Level System Settings")]
    public Slider levelBar; // Reference to the UI slider
    public int baseXPPerLevel = 10; // Base XP required for level 1
    public float xpMultiplier = 1.2f; // Multiplier for XP required for each new level
    public int xpPerGem = 1; // Amount of XP a gem gives
    private int currentXP = 0; // Current collected XP
    private int currentLevel = 1; // Current player level
    private int xpRequiredForNextLevel; // XP required for the next level

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        CalculateXPRequiredForNextLevel();
        UpdateLevelBar();
    }

    public void AddXP(int xpAmount)
    {
        currentXP += xpAmount;
        UpdateLevelBar();

        if (currentXP >= xpRequiredForNextLevel)
        {
            LevelUp();
        }
    }


    private void LevelUp()
    {
        currentXP -= xpRequiredForNextLevel; // Carry over extra XP
        currentLevel++;

        Debug.Log("Level Up! Current Level: " + currentLevel);

        CalculateXPRequiredForNextLevel();
        UpdateLevelBar();

        // Optional: Adjust difficulty, add rewards, etc.
    }

    private void CalculateXPRequiredForNextLevel()
    {
        xpRequiredForNextLevel = Mathf.RoundToInt(baseXPPerLevel * Mathf.Pow(xpMultiplier, currentLevel - 1));
    }

    private void UpdateLevelBar()
    {
        if (levelBar != null)
        {
            levelBar.maxValue = xpRequiredForNextLevel;
            levelBar.value = currentXP;
        }
    }
}