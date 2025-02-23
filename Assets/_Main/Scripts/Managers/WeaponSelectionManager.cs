using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectionManager : MonoBehaviour
{
    GameTimeManager gameTimeManager;

    public GameObject weaponSelectionPanel;
    public GameObject gameTimeText;
    public GameObject Tutorial;
    public GameObject XPLevel;
    public GameObject PlayerHP;

    [Header("Weapon Buttons")]
    public Button selectedWeaponButton;
    public TextMeshProUGUI selectedWeaponText;
    public Button pistolButton;
    public Button smgButton;
    public Button shotgunButton;
    public Button rocketButton;

    [Header("Cases")]
    public Button selectedCaseButton;
    public TextMeshProUGUI selectedCaseText;
    public Button defaultCaseButton;
    public Button secondCaseButton;
    public Button thirdCaseButton;

    private GameObject selectedWeapon;
    private GameObject selectedCase;
    private string lastAppliedCase = null; // Store the last applied case

    private void Start()
    {
        gameTimeManager = GetComponent<GameTimeManager>();
        gameTimeManager.timerRunning = false;

        if (PauseManager.instance != null)
        {
            PauseManager.instance.PauseGame();
        }

        // Set default selections
        SelectWeapon(PlayerUpgradeManager.Instance.pistol, pistolButton, "Pistol");
        SelectCase(PlayerUpgradeManager.Instance.Default, defaultCaseButton, "Default Case");

        // Add event listeners
        pistolButton.onClick.AddListener(() => SelectWeapon(PlayerUpgradeManager.Instance.pistol, pistolButton, "Pistol"));
        smgButton.onClick.AddListener(() => SelectWeapon(PlayerUpgradeManager.Instance.smg, smgButton, "SMG"));
        shotgunButton.onClick.AddListener(() => SelectWeapon(PlayerUpgradeManager.Instance.shotgun, shotgunButton, "Shotgun"));
        rocketButton.onClick.AddListener(() => SelectWeapon(PlayerUpgradeManager.Instance.rocket, rocketButton, "Rocket"));

        defaultCaseButton.onClick.AddListener(() => SelectCase(PlayerUpgradeManager.Instance.Default, defaultCaseButton, "Default Case"));
        secondCaseButton.onClick.AddListener(() => SelectCase(PlayerUpgradeManager.Instance.Second, secondCaseButton, "Second Case"));
        thirdCaseButton.onClick.AddListener(() => SelectCase(PlayerUpgradeManager.Instance.Third, thirdCaseButton, "Third Case"));
    }

    private void SelectWeapon(GameObject weapon, Button weaponButton, string weaponName)
    {
        selectedWeapon = weapon;
        selectedWeaponButton.image.sprite = weaponButton.image.sprite;
        selectedWeaponText.text = weaponName;

        AudioManager.instance.PlaySoundSFX(AudioManager.instance.UIClickSFX);
    }

    private void SelectCase(GameObject weaponCase, Button caseButton, string caseName)
    {
        // Prevent reapplying the same case
        if (lastAppliedCase == caseName) return;

        // Reset stats to default before applying a new case
        PlayerUpgradeManager.Instance.ResetCaseStats();

        selectedCase = weaponCase;
        selectedCaseButton.image.sprite = caseButton.image.sprite;
        selectedCaseButton.image.color = caseButton.image.color;
        selectedCaseText.text = caseName;

        AudioManager.instance.PlaySoundSFX(AudioManager.instance.UIClickSFX);

        // Apply only the new case's effects
        PlayerUpgradeManager.Instance.ApplyCaseStats(caseName);
        lastAppliedCase = caseName; // Store the applied case name
    }

    public void Battle()
    {
        if (PauseManager.instance != null)
        {
            PauseManager.instance.UnPauseGame();
        }

        weaponSelectionPanel.SetActive(false);
        Tutorial.SetActive(true);
        StartCoroutine(ActivateDelayed());

        // Set only the selected weapon active for the start.
        if (selectedWeapon != null)
        {
            PlayerUpgradeManager.Instance.SetStartingWeapon(selectedWeapon);
        }

        // Activate the selected case
        if (selectedCase != null)
        {
            selectedCase.SetActive(true);
        }
    }


    private IEnumerator ActivateDelayed()
    {
        yield return new WaitForSeconds(6);
        gameTimeText.SetActive(true);
        gameTimeManager.timerRunning = true;
        XPLevel.SetActive(true);
        PlayerHP.SetActive(true);
    }
}
