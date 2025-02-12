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
    public Button shotgunButton;
    public Button rocketButton;

    private GameObject selectedWeapon; // Store selected weapon

    private void Awake()
    {
        gameTimeManager = GetComponent<GameTimeManager>();
        gameTimeManager.timerRunning = false;
        PauseManager.instance.PauseGame();

        // Set default weapon (e.g., Pistol)
        selectedWeapon = UpgradeManager.Instance.Pistol;
        selectedWeaponButton.image.sprite = pistolButton.image.sprite;
        selectedWeaponText.text = "Pistol"; // Set default text

        // Add event listeners to change selected weapon
        pistolButton.onClick.AddListener(() => SelectWeapon(UpgradeManager.Instance.Pistol, pistolButton, "Pistol"));
        shotgunButton.onClick.AddListener(() => SelectWeapon(UpgradeManager.Instance.Shotgun, shotgunButton, "Shotgun"));
        rocketButton.onClick.AddListener(() => SelectWeapon(UpgradeManager.Instance.Rocket, rocketButton, "Rocket"));
    }

    private void SelectWeapon(GameObject weapon, Button weaponButton, string weaponName)
    {
        selectedWeapon = weapon;
        selectedWeaponButton.image.sprite = weaponButton.image.sprite; // Update button image
        selectedWeaponText.text = weaponName; // Update button text
    }

    public void Battle()
    {
        PauseManager.instance.UnPauseGame();
        weaponSelectionPanel.SetActive(false);
        Tutorial.SetActive(true);
        StartCoroutine(ActivateDelayed());

        // Activate the selected weapon (default or chosen)
        if (selectedWeapon != null)
        {
            selectedWeapon.SetActive(true);
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
