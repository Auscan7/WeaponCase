using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutSelectionManager : MonoBehaviour
{
    public static LoadoutSelectionManager Instance { get; private set; }
    public bool IsLoadoutActive => loadoutUI.activeSelf;
    [SerializeField] private GameObject loadoutUI;

    private GameTimeManager gameTimeManager;

    public GameObject loadoutSelectionPanel;
    public GameObject gameTimeText;
    public GameObject XPLevel;
    public GameObject PlayerHP;
    public GameObject WeaponCooldownSlots;
    [SerializeField] private CanvasGroup coolDownSlotCanvasGroup;
    [SerializeField] private CanvasGroup statScreenCanvasGroup;

    private BoatData currentLockedBoat;

    [Header("Weapon Buttons")]
    public Image selectedWeaponIconImage;
    public TextMeshProUGUI selectedWeaponText;

    public Button bowAndArrowButton;
    public Image bowAndArrowIconImage;

    public Button spearButton;
    public Image spearIconImage;

    public Button pistolButton;
    public Image pistolIconImage;

    public Button shotgunButton;
    public Image shotgunIconImage;

    [Header("Boats")]
    public Image selectedBoatIconImage;
    public TextMeshProUGUI selectedBoatText;
    public List<BoatData> boats = new();

    private GameObject selectedWeapon;
    private GameObject selectedBoat;
    private string lastAppliedBoat = null;
    private Tween notEnoughMoneyTween;

    private void Awake()
    {
        Instance = this;
        loadoutSelectionPanel.SetActive(true);
    }

    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame(); // Ensures spawners had time to register

        SpawnerManager.Instance.PauseAllSpawners();
        PlayerInputManager.instance.enabled = false; // Disable player input at the start

        gameTimeManager = GetComponent<GameTimeManager>();
        gameTimeManager.timerRunning = false;
        coolDownSlotCanvasGroup.alpha = 0f; // Hide the cooldown slots at the start
        statScreenCanvasGroup.alpha = 0f; // Hide the stat screen at the start

        // Update all lock visuals once at startup
        foreach (var boat in boats)
        {
            if (boat.lockManager != null)
            {
                boat.lockManager.UpdateLockVisuals();
            }
        }

        // Default weapon selection
        SelectWeapon(PlayerUpgradeManager.Instance.bowAndArrow, bowAndArrowButton, bowAndArrowIconImage, "Bow and Arrow", playAudio: false);

        // Default boat selection if unlocked
        //if (!boats[0].lockManager.isLocked)
        SelectBoat(boats[0], playAudio: false);

        // Weapon button setup
        bowAndArrowButton.onClick.AddListener(() =>
            SelectWeapon(PlayerUpgradeManager.Instance.bowAndArrow, bowAndArrowButton, bowAndArrowIconImage, "Bow and Arrow", playAudio: true));

        spearButton.onClick.AddListener(() =>
            SelectWeapon(PlayerUpgradeManager.Instance.spear, spearButton, spearIconImage, "Spear", playAudio: true));

        pistolButton.onClick.AddListener(() =>
            SelectWeapon(PlayerUpgradeManager.Instance.pistol, pistolButton, pistolIconImage, "Pistol", playAudio: true));

        shotgunButton.onClick.AddListener(() =>
            SelectWeapon(PlayerUpgradeManager.Instance.shotgun, shotgunButton, shotgunIconImage, "Shotgun", playAudio: true));

        // Boat button setup
        foreach (var boat in boats)
        {
            var cachedBoat = boat; // Closure safety
            boat.boatButton.onClick.AddListener(() => SelectBoat(cachedBoat, playAudio: true));
        }
    }

    // This method is called when a weapon button is clicked
    private void SelectWeapon(GameObject weapon, Button weaponButton, Image weaponIcon, string weaponName, bool playAudio)
    {
        selectedWeapon = weapon;
        selectedWeaponIconImage.sprite = weaponIcon.sprite;
        selectedWeaponText.text = weaponName;

        if (playAudio)
            AudioManager.instance.PlaySoundSFX(AudioManager.instance.UIClickSFX);
    }

    // This method is called when a boat button is clicked
    private void SelectBoat(BoatData boat, bool playAudio)
    {
        //if (lastAppliedBoat == boat.boatName)
        //    return;

        PlayerUpgradeManager.Instance.ResetBoatStats();

        selectedBoat = boat.boatObject;
        selectedBoatIconImage.sprite = boat.boatIconImage.sprite;
        selectedBoatText.text = boat.boatName;

        foreach (var b in boats)
            b.weaponPanel.SetActive(b == boat);

        if (boat.defaultWeaponPrefab != null && boat.defaultWeaponButton != null)
        {
            SelectWeapon(boat.defaultWeaponPrefab, boat.defaultWeaponButton, boat.defaultWeaponIconImage, boat.defaultWeaponName, playAudio: false);
        }

        if(playAudio)
            AudioManager.instance.PlaySoundSFX(AudioManager.instance.UIClickSFX);

        PlayerUpgradeManager.Instance.ApplyBoatStats(boat.boatName);
        PlayerStatsUIManager.Instance.UpdateStats();
        lastAppliedBoat = boat.boatName;
    }

    // This method is called when the "Unlock" button is clicked
    public void TryUnlockBoat(BoatData boat)
    {
        if (!boat.lockManager.isLocked) return;

        if (boat.lockManager.UnlockBoat())
        {
            SelectBoat(boat, playAudio: false); // Auto-select once unlocked
        }
    }

    // This method is called when the "Battle" button is clicked
    public void Battle()
    {
        AudioManager.instance.PlaySoundSFX(AudioManager.instance.UIClickSFX);

        loadoutSelectionPanel.SetActive(false);
        PlayerUpgradeManager.Instance.ActivateWeapon(selectedWeapon.name);

        if (selectedWeapon != null)
            PlayerUpgradeManager.Instance.SetStartingWeapon(selectedWeapon);

        if (selectedBoat != null)
            selectedBoat.SetActive(true);

        PlayerInputManager.instance.enabled = true;
        SpawnerManager.Instance.ResumeAllSpawners();
        gameTimeText.SetActive(true);
        gameTimeManager.timerRunning = true;
        XPLevel.SetActive(true);
        PlayerHP.SetActive(true);
        coolDownSlotCanvasGroup.alpha = 1f;
        statScreenCanvasGroup.alpha = 1f;
    }
}

[System.Serializable]
public class BoatData
{
    public LockManager lockManager;

    public string boatName;
    public GameObject boatObject;
    public Button boatButton;
    public Image boatIconImage;
    public GameObject weaponPanel;

    [Header("Default Weapon")]
    public GameObject defaultWeaponPrefab; // The default weapon prefab for this boat
    public Button defaultWeaponButton; // The button to select the default weapon
    public Image defaultWeaponIconImage; // The icon image for the default weapon
    public string defaultWeaponName; // The name of the default weapon
}
