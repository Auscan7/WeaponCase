using System.Collections;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("Weapons")]
    public GameObject Pistol;
    public GameObject Shotgun;
    public GameObject Rocket;

    [Header("Weapon Damages")]
    public float pistolDamage;
    public float shotgunDamage;
    public float rocketDamage;
    public float rocketAreaDamage;

    [Header("Weapon Firerates")]
    public float pistolFirerate;
    public float shotgunFirerate;
    public float rocketFirerate;


    public static WeaponManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        // Subscribe to the event to listen for upgrade selections
        UpgradeSelectionScript.OnUpgradeSelectedEvent += ActivateWeapons;
        UpgradeSelectionScript.OnUpgradeSelectedEvent += IncreaseDamage;
        UpgradeSelectionScript.OnUpgradeSelectedEvent += Gunpowder;
        UpgradeSelectionScript.OnUpgradeSelectedEvent += ExtraMagazine;
    }

    private void OnDisable()
    {
        // Unsubscribe when the object is disabled or destroyed to avoid memory leaks
        UpgradeSelectionScript.OnUpgradeSelectedEvent -= ActivateWeapons;
        UpgradeSelectionScript.OnUpgradeSelectedEvent -= IncreaseDamage;
        UpgradeSelectionScript.OnUpgradeSelectedEvent -= Gunpowder;
        UpgradeSelectionScript.OnUpgradeSelectedEvent -= ExtraMagazine;
    }

    private void ActivateWeapons(int selectedUpgradeID)
    {
        // Activate the selected weapon based on the upgradeID
        if (selectedUpgradeID == 2)
        {
            Pistol.SetActive(true);
            pistolDamage = pistolDamage + (pistolDamage / 10) * 2.5f;
        }
        else if (selectedUpgradeID == 4)
        {
            Shotgun.SetActive(true);
            shotgunDamage = shotgunDamage + (shotgunDamage / 10) * 2f;
        }
        else if (selectedUpgradeID == 3)
        {
            Rocket.SetActive(true);
            rocketDamage = rocketDamage + (rocketDamage / 10) * 4f;
            rocketAreaDamage = rocketAreaDamage + (rocketAreaDamage / 10) * 1.5f;
        }
    }

    private void IncreaseDamage(int selectedUpgradeID)
    {
        float damageUpgradePercentage = 1;

        if (selectedUpgradeID == 0)
        {
            pistolDamage = pistolDamage + (pistolDamage / 10) * damageUpgradePercentage;

            shotgunDamage = shotgunDamage + (shotgunDamage / 10) * damageUpgradePercentage;

            rocketDamage = rocketDamage + (rocketDamage / 10) * damageUpgradePercentage;
            rocketAreaDamage = rocketAreaDamage + (rocketAreaDamage / 10) * damageUpgradePercentage;
        }
    }

    private void Gunpowder(int selectedUpgradeID)
    {
        float damageUpgradePercentage = 2;
        float fireRateDecreasePercentage = 1;

        if (selectedUpgradeID == 5)
        {
            pistolDamage = pistolDamage + (pistolDamage / 10) * damageUpgradePercentage;
            pistolFirerate = pistolFirerate - (pistolFirerate / 10) * fireRateDecreasePercentage;

            shotgunDamage = shotgunDamage + (shotgunDamage / 10) * damageUpgradePercentage;
            shotgunFirerate = shotgunFirerate - (shotgunFirerate / 10) * fireRateDecreasePercentage;

            rocketDamage = rocketDamage + (rocketDamage / 10) * damageUpgradePercentage;
            rocketAreaDamage = rocketAreaDamage + (rocketAreaDamage / 10) * damageUpgradePercentage;
            rocketFirerate = rocketFirerate - (rocketFirerate / 10) * fireRateDecreasePercentage;
        }
    }

    private void ExtraMagazine(int selectedUpgradeID)
    {
        float fireRatePercentage = 1;

        if (selectedUpgradeID == 1)
        {
            pistolFirerate = pistolFirerate + (pistolFirerate / 10) * fireRatePercentage;

            shotgunFirerate = shotgunFirerate + (shotgunFirerate / 10) * fireRatePercentage;

            rocketFirerate = rocketFirerate + (rocketFirerate / 10) * fireRatePercentage;
        }
    }
}
