using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Collections;

public class PlayerLevelSystem : MonoBehaviour
{
    [Header("Weapon Offer Quota Settings")]
    public int weaponQuotaWindow = 9; // Check weapon offers in first 10 levels
    public int minWeaponOffersInQuota = 2; // Must offer at least 3 in first 10 levels
    [HideInInspector] public int currentWeaponOffers = 0;
    [HideInInspector] public int levelsCheckedForWeapon = 0;

    public int CurrentLevel => currentLevel;

    public static PlayerLevelSystem instance;
    [SerializeField]private Transform playerTransform;

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
        currentLevelText.text = "Lvl " + currentLevel.ToString();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    LevelUp();
        //}

        //if (Input.GetKeyDown(KeyCode.O))
        //{
        //    if (upgradeSelection != null)
        //    {
        //        upgradeSelection.SetUpgradeCards();
        //    }
        //}

        if (waitForKeyPress && Input.anyKeyDown)
        {
            PauseManager.instance.UnPauseGame();
            // to do - remmove press any key text
            waitForKeyPress = false;
        }
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

        if (currentLevel > weaponQuotaWindow)
        {
            currentWeaponOffers = 0;
            levelsCheckedForWeapon = 0;
        }

        currentLevelText.text = "Lvl " + currentLevel.ToString();

        CalculateXPRequiredForNextLevel();
        UpdateLevelBar();

        EffectsManager.instance.PlayVFX(EffectsManager.instance.levelUpVFX, playerTransform.position, Quaternion.identity);
        AudioManager.instance.PlaySoundSFX(AudioManager.instance.LevelUpSFX);
        StartCoroutine(ShowUpgradeScreen());
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

    private IEnumerator ShowUpgradeScreen()
    {
        yield return new WaitForSeconds(0.55f); // Wait for a short duration before showing the upgrade screen
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
}
