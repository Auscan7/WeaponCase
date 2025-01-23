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
    }

    private void OnDisable()
    {
        // Unsubscribe when the object is disabled or destroyed to avoid memory leaks
        UpgradeSelectionScript.OnUpgradeSelectedEvent -= ActivateWeapons;
        UpgradeSelectionScript.OnUpgradeSelectedEvent -= IncreaseDamage;
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
}
