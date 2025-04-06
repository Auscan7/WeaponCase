using System.Collections;
using System.Collections.Generic;
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

    [Header("Boats")]
    public Button selectedBoatButton;
    public TextMeshProUGUI selectedBoatText;
    public List<BoatData> boats = new List<BoatData>();

    private GameObject selectedWeapon;
    private GameObject selectedBoat;
    private string lastAppliedBoat = null;

    private void Start()
    {
        gameTimeManager = GetComponent<GameTimeManager>();
        gameTimeManager.timerRunning = false;

        if (PauseManager.instance != null)
            PauseManager.instance.PauseGame();

        // Default selections
        SelectWeapon(PlayerUpgradeManager.Instance.pistol, pistolButton, "Pistol");
        SelectBoat(boats[0]);

        // Weapon button listeners
        pistolButton.onClick.AddListener(() => SelectWeapon(PlayerUpgradeManager.Instance.pistol, pistolButton, "Pistol"));
        smgButton.onClick.AddListener(() => SelectWeapon(PlayerUpgradeManager.Instance.smg, smgButton, "SMG"));
        shotgunButton.onClick.AddListener(() => SelectWeapon(PlayerUpgradeManager.Instance.shotgun, shotgunButton, "Shotgun"));
        rocketButton.onClick.AddListener(() => SelectWeapon(PlayerUpgradeManager.Instance.rocket, rocketButton, "Rocket"));

        // Boat button listeners
        foreach (var boat in boats)
        {
            var cachedBoat = boat; // avoid modified closure issue
            boat.boatButton.onClick.AddListener(() => SelectBoat(cachedBoat));
        }
    }

    private void SelectWeapon(GameObject weapon, Button weaponButton, string weaponName)
    {
        selectedWeapon = weapon;
        selectedWeaponButton.image.sprite = weaponButton.image.sprite;
        selectedWeaponText.text = weaponName;
        AudioManager.instance.PlaySoundSFX(AudioManager.instance.UIClickSFX);
    }

    private void SelectBoat(BoatData boat)
    {
        if (lastAppliedBoat == boat.boatName) return;

        PlayerUpgradeManager.Instance.ResetBoatStats();

        selectedBoat = boat.boatObject;
        selectedBoatButton.image.sprite = boat.boatButton.image.sprite;
        selectedBoatButton.image.color = boat.boatButton.image.color;
        selectedBoatText.text = boat.boatName;

        foreach (var b in boats)
        {
            b.weaponPanel.SetActive(b == boat);
        }

        AudioManager.instance.PlaySoundSFX(AudioManager.instance.UIClickSFX);
        PlayerUpgradeManager.Instance.ApplyBoatStats(boat.boatName);
        lastAppliedBoat = boat.boatName;
    }

    public void Battle()
    {
        if (PauseManager.instance != null)
            PauseManager.instance.UnPauseGame();

        weaponSelectionPanel.SetActive(false);
        Tutorial.SetActive(true);
        StartCoroutine(ActivateDelayed());

        if (selectedWeapon != null)
            PlayerUpgradeManager.Instance.SetStartingWeapon(selectedWeapon);

        if (selectedBoat != null)
            selectedBoat.SetActive(true);
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


[System.Serializable]
public class BoatData
{
    public string boatName;
    public GameObject boatObject;
    public Button boatButton;
    public GameObject weaponPanel;
}
