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
            upgradeManager.bowAndArrow,
            upgradeManager.spear,
            upgradeManager.pistol,
            upgradeManager.shotgun
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

    public void MountWeapons()
    {
        // Get all weapons in the scene
        Weapon[] weapons = FindObjectsOfType<Weapon>();

        foreach (Weapon weapon in weapons)
        {
            if (!weapon.shouldMountToSlot) continue; // Skip weapons that shouldn't be mounted

            // Check if the weapon is already mounted
            if (weapon.transform.parent != null && 
                (weapon.transform.parent == slotManager.manualSlot || 
                 slotManager.usedAutoSlots.Contains(weapon.transform.parent)))
            {
                continue;
            }

            // Mount the weapon
            slotManager.AttachWeapon(weapon);
        }
    }
}
