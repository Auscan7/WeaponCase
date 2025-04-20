using UnityEngine;
using System.Collections.Generic;

public class AutoMountWeapons : MonoBehaviour
{
    private WeaponSlotManager slotManager;
    private PlayerUpgradeManager upgradeManager;

    void OnEnable()
    {
        // Subscribe to the event
        GameEvents.OnWeaponActivated += HandleWeaponActivation;

        // Mount any already-active weapons on startup
        MountInitialWeapons();
    }

    void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        GameEvents.OnWeaponActivated -= HandleWeaponActivation;
    }

    void Awake()
    {
        slotManager = GetComponent<WeaponSlotManager>();
        upgradeManager = GetComponent<PlayerUpgradeManager>();

        if (slotManager == null || upgradeManager == null)
        {
            Debug.LogError("Missing WeaponSlotManager or PlayerUpgradeManager component.");
            return;
        }
    }

    private void MountInitialWeapons()
    {
        List<GameObject> allWeapons = new List<GameObject>
        {
            upgradeManager.pistol,
            upgradeManager.bowAndArrow,
            upgradeManager.spear,
            upgradeManager.blowDart,
            upgradeManager.slingShot,
            upgradeManager.smg,
            upgradeManager.shotgun,
            upgradeManager.rocket
            //upgradeManager.orbitalStrike,
            //upgradeManager.grenade
        };

        foreach (GameObject weaponGO in allWeapons)
        {
            if (weaponGO.activeSelf)
            {
                // Fire the event for already-active weapons
                GameEvents.WeaponActivated(weaponGO);
            }
        }
    }

    private void HandleWeaponActivation(GameObject weaponGO)
    {
        if (slotManager == null)
        {
            Debug.LogError("AutoMountWeapons: slotManager is null during HandleWeaponActivation.");
            return;
        }

        Weapon weapon = weaponGO.GetComponent<Weapon>();
        if (weapon != null)
        {
            slotManager.AttachWeapon(weapon);
        }
        else
        {
            Debug.LogWarning($"{weaponGO.name} is missing a Weapon component!");
        }
    }

}
