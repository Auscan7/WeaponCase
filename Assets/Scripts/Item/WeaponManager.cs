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
    }

    private void OnDisable()
    {
        // Unsubscribe when the object is disabled or destroyed to avoid memory leaks
        UpgradeSelectionScript.OnUpgradeSelectedEvent -= ActivateWeapons;
    }

    private void ActivateWeapons(int selectedUpgradeID)
    {
        // Activate the selected weapon based on the upgradeID
        if (selectedUpgradeID == 2)
        {
            Pistol.SetActive(true);
            pistolDamage = pistolDamage + (pistolDamage * 1.2f) / 10;
        }
        else if (selectedUpgradeID == 4)
        {
            Shotgun.SetActive(true);
        }
        else if (selectedUpgradeID == 3)
        {
            Rocket.SetActive(true);
        }
    }

    private void IncreadeDamage(int selectedUpgradeID)
    {

    }
}
