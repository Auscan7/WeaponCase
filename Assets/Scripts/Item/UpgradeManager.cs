using TMPro;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [Header("Player Stats")]
    public float playercurrentHealth;
    public int playerMaxHealth;
    public float playerMovementSpeedMultiplier = 1;

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


    public static UpgradeManager Instance;

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

        // Polymer Tip
        UpgradeSelectionScript.OnUpgradeSelectedEvent += PolymerTip1;
        UpgradeSelectionScript.OnUpgradeSelectedEvent += PolymerTip2;
        UpgradeSelectionScript.OnUpgradeSelectedEvent += PolymerTip3;
        UpgradeSelectionScript.OnUpgradeSelectedEvent += PolymerTip4;
        UpgradeSelectionScript.OnUpgradeSelectedEvent += PolymerTip5;

        // Gunpowder
        UpgradeSelectionScript.OnUpgradeSelectedEvent += Gunpowder1;
        UpgradeSelectionScript.OnUpgradeSelectedEvent += Gunpowder2;
        UpgradeSelectionScript.OnUpgradeSelectedEvent += Gunpowder3;
        UpgradeSelectionScript.OnUpgradeSelectedEvent += Gunpowder4;
        UpgradeSelectionScript.OnUpgradeSelectedEvent += Gunpowder5;

        // Extra Magazine
        UpgradeSelectionScript.OnUpgradeSelectedEvent += ExtraMagazine1;
        UpgradeSelectionScript.OnUpgradeSelectedEvent += ExtraMagazine2;
        UpgradeSelectionScript.OnUpgradeSelectedEvent += ExtraMagazine3;
        UpgradeSelectionScript.OnUpgradeSelectedEvent += ExtraMagazine4;
        UpgradeSelectionScript.OnUpgradeSelectedEvent += ExtraMagazine5;

        // Steel Plates
        UpgradeSelectionScript.OnUpgradeSelectedEvent += SteelPlates1;
        UpgradeSelectionScript.OnUpgradeSelectedEvent += SteelPlates2;
        UpgradeSelectionScript.OnUpgradeSelectedEvent += SteelPlates3;
        UpgradeSelectionScript.OnUpgradeSelectedEvent += SteelPlates4;
        UpgradeSelectionScript.OnUpgradeSelectedEvent += SteelPlates5;

        // Turbines
        UpgradeSelectionScript.OnUpgradeSelectedEvent += Turbines1;
        UpgradeSelectionScript.OnUpgradeSelectedEvent += Turbines2;
        UpgradeSelectionScript.OnUpgradeSelectedEvent += Turbines3;
        UpgradeSelectionScript.OnUpgradeSelectedEvent += Turbines4;
        UpgradeSelectionScript.OnUpgradeSelectedEvent += Turbines5;
    }

    private void OnDisable()
    {
        // Unsubscribe when the object is disabled or destroyed to avoid memory leaks
        UpgradeSelectionScript.OnUpgradeSelectedEvent -= ActivateWeapons;

        // Polymer Tip
        UpgradeSelectionScript.OnUpgradeSelectedEvent -= PolymerTip1;
        UpgradeSelectionScript.OnUpgradeSelectedEvent -= PolymerTip2;
        UpgradeSelectionScript.OnUpgradeSelectedEvent -= PolymerTip3;
        UpgradeSelectionScript.OnUpgradeSelectedEvent -= PolymerTip4;
        UpgradeSelectionScript.OnUpgradeSelectedEvent -= PolymerTip5;

        // Gunpowder
        UpgradeSelectionScript.OnUpgradeSelectedEvent -= Gunpowder1;
        UpgradeSelectionScript.OnUpgradeSelectedEvent -= Gunpowder2;
        UpgradeSelectionScript.OnUpgradeSelectedEvent -= Gunpowder3;
        UpgradeSelectionScript.OnUpgradeSelectedEvent -= Gunpowder4;
        UpgradeSelectionScript.OnUpgradeSelectedEvent -= Gunpowder5;

        // Extra Magazine
        UpgradeSelectionScript.OnUpgradeSelectedEvent -= ExtraMagazine1;
        UpgradeSelectionScript.OnUpgradeSelectedEvent -= ExtraMagazine2;
        UpgradeSelectionScript.OnUpgradeSelectedEvent -= ExtraMagazine3;
        UpgradeSelectionScript.OnUpgradeSelectedEvent -= ExtraMagazine4;
        UpgradeSelectionScript.OnUpgradeSelectedEvent -= ExtraMagazine5;

        // Steel Plates
        UpgradeSelectionScript.OnUpgradeSelectedEvent -= SteelPlates1;
        UpgradeSelectionScript.OnUpgradeSelectedEvent -= SteelPlates2;
        UpgradeSelectionScript.OnUpgradeSelectedEvent -= SteelPlates3;
        UpgradeSelectionScript.OnUpgradeSelectedEvent -= SteelPlates4;
        UpgradeSelectionScript.OnUpgradeSelectedEvent -= SteelPlates5;

        // Turbines
        UpgradeSelectionScript.OnUpgradeSelectedEvent -= Turbines1;
        UpgradeSelectionScript.OnUpgradeSelectedEvent -= Turbines2;
        UpgradeSelectionScript.OnUpgradeSelectedEvent -= Turbines3;
        UpgradeSelectionScript.OnUpgradeSelectedEvent -= Turbines4;
        UpgradeSelectionScript.OnUpgradeSelectedEvent -= Turbines5;
    }

    private void ActivateWeapons(int selectedUpgradeID)
    {
        // Activate the selected weapon based on the upgradeID

        //Pistol
        if (selectedUpgradeID == 1000)
        {
            Pistol.SetActive(true);
            pistolDamage = pistolDamage + (pistolDamage / 10) * 2.5f;
        }
        else if (selectedUpgradeID == 1001)
        {
            Pistol.SetActive(true);
            pistolDamage = pistolDamage + (pistolDamage / 10) * 4f;
        }
        else if (selectedUpgradeID == 1002)
        {
            Pistol.SetActive(true);
            pistolDamage = pistolDamage + (pistolDamage / 10) * 5.5f;
        }
        else if (selectedUpgradeID == 1003)
        {
            Pistol.SetActive(true);
            pistolDamage = pistolDamage + (pistolDamage / 10) * 7f;
        }
        else if (selectedUpgradeID == 1004)
        {
            Pistol.SetActive(true);
            pistolDamage = pistolDamage + (pistolDamage / 10) * 12.5f;
        }

        //Shotgun
        if (selectedUpgradeID == 1005)
        {
            Shotgun.SetActive(true);
            shotgunDamage = shotgunDamage + (shotgunDamage / 10) * 2f;
        }
        else if (selectedUpgradeID == 1006)
        {
            Shotgun.SetActive(true);
            shotgunDamage = shotgunDamage + (shotgunDamage / 10) * 3f;
        }
        else if (selectedUpgradeID == 1007)
        {
            Shotgun.SetActive(true);
            shotgunDamage = shotgunDamage + (shotgunDamage / 10) * 4f;
        }
        else if (selectedUpgradeID == 1008)
        {
            Shotgun.SetActive(true);
            shotgunDamage = shotgunDamage + (shotgunDamage / 10) * 5f;
        }
        else if (selectedUpgradeID == 1009)
        {
            Shotgun.SetActive(true);
            shotgunDamage = shotgunDamage + (shotgunDamage / 10) * 7.5f;
        }

        //Rocket
        if (selectedUpgradeID == 1010)
        {
            Rocket.SetActive(true);
            rocketDamage = rocketDamage + (rocketDamage / 10) * 3f;
            rocketAreaDamage = rocketAreaDamage + (rocketAreaDamage / 10) * 1f;
        }
        else if (selectedUpgradeID == 1011)
        {
            Rocket.SetActive(true);
            rocketDamage = rocketDamage + (rocketDamage / 10) * 4f;
            rocketAreaDamage = rocketAreaDamage + (rocketAreaDamage / 10) * 1.5f;
        }
        else if (selectedUpgradeID == 1012)
        {
            Rocket.SetActive(true);
            rocketDamage = rocketDamage + (rocketDamage / 10) * 5f;
            rocketAreaDamage = rocketAreaDamage + (rocketAreaDamage / 10) * 2f;
        }
        else if (selectedUpgradeID == 1013)
        {
            Rocket.SetActive(true);
            rocketDamage = rocketDamage + (rocketDamage / 10) * 6f;
            rocketAreaDamage = rocketAreaDamage + (rocketAreaDamage / 10) * 2.5f;
        }
        else if (selectedUpgradeID == 1014)
        {
            Rocket.SetActive(true);
            rocketDamage = rocketDamage + (rocketDamage / 10) * 12f;
            rocketAreaDamage = rocketAreaDamage + (rocketAreaDamage / 10) * 5f;
        }
    }

    // Polymer Tip
    private void PolymerTip1(int selectedUpgradeID)
    {
        float damageUpgradePercentage = 1.5f;

        if (selectedUpgradeID == 0)
        {
            pistolDamage = pistolDamage + (pistolDamage / 10) * damageUpgradePercentage;

            shotgunDamage = shotgunDamage + (shotgunDamage / 10) * damageUpgradePercentage;

            rocketDamage = rocketDamage + (rocketDamage / 10) * damageUpgradePercentage;
            rocketAreaDamage = rocketAreaDamage + (rocketAreaDamage / 10) * damageUpgradePercentage;
        }
    }    
    
    private void PolymerTip2(int selectedUpgradeID)
    {
        float damageUpgradePercentage = 2.5f;

        if (selectedUpgradeID == 1)
        {
            pistolDamage = pistolDamage + (pistolDamage / 10) * damageUpgradePercentage;

            shotgunDamage = shotgunDamage + (shotgunDamage / 10) * damageUpgradePercentage;

            rocketDamage = rocketDamage + (rocketDamage / 10) * damageUpgradePercentage;
            rocketAreaDamage = rocketAreaDamage + (rocketAreaDamage / 10) * damageUpgradePercentage;
        }
    }

    private void PolymerTip3(int selectedUpgradeID)
    {
        float damageUpgradePercentage = 3.5f;

        if (selectedUpgradeID == 2)
        {
            pistolDamage = pistolDamage + (pistolDamage / 10) * damageUpgradePercentage;

            shotgunDamage = shotgunDamage + (shotgunDamage / 10) * damageUpgradePercentage;

            rocketDamage = rocketDamage + (rocketDamage / 10) * damageUpgradePercentage;
            rocketAreaDamage = rocketAreaDamage + (rocketAreaDamage / 10) * damageUpgradePercentage;
        }
    }

    private void PolymerTip4(int selectedUpgradeID)
    {
        float damageUpgradePercentage = 4.5f;

        if (selectedUpgradeID == 3)
        {
            pistolDamage = pistolDamage + (pistolDamage / 10) * damageUpgradePercentage;

            shotgunDamage = shotgunDamage + (shotgunDamage / 10) * damageUpgradePercentage;

            rocketDamage = rocketDamage + (rocketDamage / 10) * damageUpgradePercentage;
            rocketAreaDamage = rocketAreaDamage + (rocketAreaDamage / 10) * damageUpgradePercentage;
        }
    }

    private void PolymerTip5(int selectedUpgradeID)
    {
        float damageUpgradePercentage = 7.5f;

        if (selectedUpgradeID == 4)
        {
            pistolDamage = pistolDamage + (pistolDamage / 10) * damageUpgradePercentage;

            shotgunDamage = shotgunDamage + (shotgunDamage / 10) * damageUpgradePercentage;

            rocketDamage = rocketDamage + (rocketDamage / 10) * damageUpgradePercentage;
            rocketAreaDamage = rocketAreaDamage + (rocketAreaDamage / 10) * damageUpgradePercentage;
        }
    }

    // Gunpowder
    private void Gunpowder1(int selectedUpgradeID)
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

    private void Gunpowder2(int selectedUpgradeID)
    {
        float damageUpgradePercentage = 3.5f;
        float fireRateDecreasePercentage = 1;

        if (selectedUpgradeID == 6)
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

    private void Gunpowder3(int selectedUpgradeID)
    {
        float damageUpgradePercentage = 5;
        float fireRateDecreasePercentage = 1;

        if (selectedUpgradeID == 7)
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

    private void Gunpowder4(int selectedUpgradeID)
    {
        float damageUpgradePercentage = 6.5f;
        float fireRateDecreasePercentage = 1;

        if (selectedUpgradeID == 8)
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

    private void Gunpowder5(int selectedUpgradeID)
    {
        float damageUpgradePercentage = 10;
        float fireRateDecreasePercentage = 1;

        if (selectedUpgradeID == 9)
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

    // Extra Magazine
    private void ExtraMagazine1(int selectedUpgradeID)
    {
        float fireRatePercentage = 1;

        if (selectedUpgradeID == 10)
        {
            pistolFirerate = pistolFirerate + (pistolFirerate / 10) * fireRatePercentage;

            shotgunFirerate = shotgunFirerate + (shotgunFirerate / 10) * fireRatePercentage;

            rocketFirerate = rocketFirerate + (rocketFirerate / 10) * fireRatePercentage;
        }
    }

    private void ExtraMagazine2(int selectedUpgradeID)
    {
        float fireRatePercentage = 1.5f;

        if (selectedUpgradeID == 11)
        {
            pistolFirerate = pistolFirerate + (pistolFirerate / 10) * fireRatePercentage;

            shotgunFirerate = shotgunFirerate + (shotgunFirerate / 10) * fireRatePercentage;

            rocketFirerate = rocketFirerate + (rocketFirerate / 10) * fireRatePercentage;
        }
    }

    private void ExtraMagazine3(int selectedUpgradeID)
    {
        float fireRatePercentage = 2f;

        if (selectedUpgradeID == 12)
        {
            pistolFirerate = pistolFirerate + (pistolFirerate / 10) * fireRatePercentage;

            shotgunFirerate = shotgunFirerate + (shotgunFirerate / 10) * fireRatePercentage;

            rocketFirerate = rocketFirerate + (rocketFirerate / 10) * fireRatePercentage;
        }
    }

    private void ExtraMagazine4(int selectedUpgradeID)
    {
        float fireRatePercentage = 2.5f;

        if (selectedUpgradeID == 13)
        {
            pistolFirerate = pistolFirerate + (pistolFirerate / 10) * fireRatePercentage;

            shotgunFirerate = shotgunFirerate + (shotgunFirerate / 10) * fireRatePercentage;

            rocketFirerate = rocketFirerate + (rocketFirerate / 10) * fireRatePercentage;
        }
    }

    private void ExtraMagazine5(int selectedUpgradeID)
    {
        float fireRatePercentage = 4f;

        if (selectedUpgradeID == 14)
        {
            pistolFirerate = pistolFirerate + (pistolFirerate / 10) * fireRatePercentage;

            shotgunFirerate = shotgunFirerate + (shotgunFirerate / 10) * fireRatePercentage;

            rocketFirerate = rocketFirerate + (rocketFirerate / 10) * fireRatePercentage;
        }
    }

    // Steel Plates
    private void SteelPlates1(int selectedUpgradeID)
    {
        int extraHealth = 3;

        if (selectedUpgradeID == 15)
        {
            playerMaxHealth += extraHealth;
            playercurrentHealth += extraHealth;
        }
    }

    private void SteelPlates2(int selectedUpgradeID)
    {
        int extraHealth = 6;

        if (selectedUpgradeID == 16)
        {
            playerMaxHealth += extraHealth;
            playercurrentHealth += extraHealth;
        }
    }

    private void SteelPlates3(int selectedUpgradeID)
    {
        int extraHealth = 9;

        if (selectedUpgradeID == 17)
        {
            playerMaxHealth += extraHealth;
            playercurrentHealth += extraHealth;
        }
    }

    private void SteelPlates4(int selectedUpgradeID)
    {
        int extraHealth = 12;

        if (selectedUpgradeID == 18)
        {
            playerMaxHealth += extraHealth;
            playercurrentHealth += extraHealth;
        }
    }

    private void SteelPlates5(int selectedUpgradeID)
    {
        int extraHealth = 25;

        if (selectedUpgradeID == 19)
        {
            playerMaxHealth += extraHealth;
            playercurrentHealth += extraHealth;
        }
    }

    // Turbines
    private void Turbines1(int selectedUpgradeID)
    {
        float extraMovementSpeedPercentage = 1;

        if (selectedUpgradeID == 20)
        {
            playerMovementSpeedMultiplier = playerMovementSpeedMultiplier + (playerMovementSpeedMultiplier / 10) * extraMovementSpeedPercentage;
        }
    }

    private void Turbines2(int selectedUpgradeID)
    {
        float extraMovementSpeedPercentage = 1.2f;

        if (selectedUpgradeID == 21)
        {
            playerMovementSpeedMultiplier = playerMovementSpeedMultiplier + (playerMovementSpeedMultiplier / 10) * extraMovementSpeedPercentage;
        }
    }

    private void Turbines3(int selectedUpgradeID)
    {
        float extraMovementSpeedPercentage = 1.4f;

        if (selectedUpgradeID == 22)
        {
            playerMovementSpeedMultiplier = playerMovementSpeedMultiplier + (playerMovementSpeedMultiplier / 10) * extraMovementSpeedPercentage;
        }
    }

    private void Turbines4(int selectedUpgradeID)
    {
        float extraMovementSpeedPercentage = 1.6f;

        if (selectedUpgradeID == 23)
        {
            playerMovementSpeedMultiplier = playerMovementSpeedMultiplier + (playerMovementSpeedMultiplier / 10) * extraMovementSpeedPercentage;
        }
    }

    private void Turbines5(int selectedUpgradeID)
    {
        float extraMovementSpeedPercentage = 2.5f;

        if (selectedUpgradeID == 24)
        {
            playerMovementSpeedMultiplier = playerMovementSpeedMultiplier + (playerMovementSpeedMultiplier / 10) * extraMovementSpeedPercentage;
        }
    }
}
