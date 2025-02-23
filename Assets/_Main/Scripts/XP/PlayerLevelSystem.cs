using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

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
    public List<int> customXPRequirements; // Manually define XP for levels
    public GameObject upgradeScreen;
    private bool isUpgradeScreenActive = false;
    private bool waitForKeyPress = false;
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
        currentXP += Mathf.RoundToInt(xpAmount * PlayerUpgradeManager.Instance.playerXpMultiplier);
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
        currentLevelText.text = "Level: " + currentLevel.ToString();

        CalculateXPRequiredForNextLevel();
        UpdateLevelBar();

        ShowUpgradeScreen();
    }

    private void CalculateXPRequiredForNextLevel()
    {
        if (customXPRequirements != null && customXPRequirements.Count >= currentLevel)
        {
            xpRequiredForNextLevel = customXPRequirements[currentLevel - 1];
        }
        else
        {
            xpRequiredForNextLevel = Mathf.RoundToInt(baseXPPerLevel * Mathf.Pow(xpMultiplier, currentLevel - 1));
        }
    }

    private void UpdateLevelBar()
    {
        if (levelBar != null)
        {
            levelBar.fillAmount = (float)currentXP / xpRequiredForNextLevel;
        }
    }

    private void ShowUpgradeScreen()
    {
        if (upgradeScreen != null && !isUpgradeScreenActive)
        {
            upgradeScreen.SetActive(true);

            if (upgradeSelection != null)
            {
                upgradeSelection.SetUpgradeCards();
            }

            PauseManager.instance.PauseGame();
            isUpgradeScreenActive = true;
            waitForKeyPress = false;
        }
    }

    public void CloseUpgradeScreen()
    {
        if (upgradeScreen != null)
        {
            AudioManager.instance.PlaySoundSFX(AudioManager.instance.UIClickSFX);
            upgradeScreen.SetActive(false);
            isUpgradeScreenActive = false;
            // to do - add press any key text
            waitForKeyPress = true;
        }
    }

    private void Update()
    {
        if (waitForKeyPress && Input.anyKeyDown)
        {
            PauseManager.instance.UnPauseGame();
            // to do - remmove press any key text
            waitForKeyPress = false;
        }
    }
}
