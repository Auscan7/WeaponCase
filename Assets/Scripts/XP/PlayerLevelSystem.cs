using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLevelSystem : MonoBehaviour
{
    public static PlayerLevelSystem instance;

    [Header("Level System Settings")]
    public Image levelBar;
    public TMP_Text currentLevelText;
    public int baseXPPerLevel = 10;
    public float xpMultiplier = 1.2f;
    public int xpPerGem = 1;
    private int currentXP = 0;
    private int currentLevel = 1;
    private int xpRequiredForNextLevel;
    public GameObject upgradeScreen; // Reference to the upgrade screen (UI)
    private bool isUpgradeScreenActive = false; // Flag to check if upgrade screen is active
    private bool waitForKeyPress = false; // Flag to wait for a key press to resume the game
    UpgradeSelectionScript upgradeSelection;


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

        upgradeSelection = GetComponent<UpgradeSelectionScript>();
    }

    private void Start()
    {
        CalculateXPRequiredForNextLevel();
        UpdateLevelBar();
        currentLevelText.text = "Level: " + currentLevel.ToString();
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
        currentXP -= xpRequiredForNextLevel;
        currentLevel++;
        currentLevelText.text ="Level: " + currentLevel.ToString();

        Debug.Log("Level Up! Current Level: " + currentLevel);

        CalculateXPRequiredForNextLevel();
        UpdateLevelBar();

        ShowUpgradeScreen();
    }

    private void CalculateXPRequiredForNextLevel()
    {
        xpRequiredForNextLevel = Mathf.RoundToInt(baseXPPerLevel * Mathf.Pow(xpMultiplier, currentLevel - 1));
    }

    private void UpdateLevelBar()
    {
        if (levelBar != null)
        {
            levelBar.fillAmount = (float)currentXP / xpRequiredForNextLevel;
        }
    }

    // Show upgrade screen and pause game
    private void ShowUpgradeScreen()
    {
        if (upgradeScreen != null && !isUpgradeScreenActive)
        {
            upgradeScreen.SetActive(true); // Show the upgrade screen

            // Call SetUpgradeCards on UpgradeSelectionScript
            if (upgradeSelection != null)
            {
                Debug.Log("assigning..");  // Add this line to verify
                upgradeSelection.SetUpgradeCards(); // Refresh upgrades
            }

            PauseManager.instance.PauseGame();
            isUpgradeScreenActive = true;
            waitForKeyPress = false; // Reset wait flag
        }
    }


    // Close upgrade screen and pause until key press
    public void CloseUpgradeScreen()
    {
        if (upgradeScreen != null)
        {
            AudioManager.instance.PlaySoundSFX(AudioManager.instance.UIClickSFX);
            upgradeScreen.SetActive(false); // Hide the upgrade screen
            isUpgradeScreenActive = false;
            waitForKeyPress = true; // Wait for player input
        }
    }

    private void Update()
    {
        // Check for key press to resume game
        if (waitForKeyPress && Input.anyKeyDown)
        {
            ResumeGame();
        }
    }

    // Resume the game
    private void ResumeGame()
    {
        PauseManager.instance.UnPauseGame();
        waitForKeyPress = false; // Reset wait flag
    }
}