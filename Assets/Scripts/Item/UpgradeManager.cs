using UnityEngine;
using System.Collections.Generic;

public class UpgradeManager : MonoBehaviour
{
    [Header("Player Stats")]
    public float playerCurrentHealth;
    public int playerMaxHealth;
    public float playerMovementSpeedMultiplier = 1;

    [Header("Weapons")]
    public GameObject Pistol;
    public GameObject Shotgun;
    public GameObject Rocket;

    [Header("Weapon Stats")]
    public WeaponStats pistolStats;
    public WeaponStats shotgunStats;
    public WeaponStats rocketStats;

    public static UpgradeManager Instance;

    private Dictionary<int, System.Action> upgradeEffects;

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

        InitializeUpgradeEffects();
    }

    private void OnEnable()
    {
        UpgradeSelectionScript.OnUpgradeSelectedEvent += ApplyUpgrade;
    }

    private void OnDisable()
    {
        UpgradeSelectionScript.OnUpgradeSelectedEvent -= ApplyUpgrade;
    }

    private void InitializeUpgradeEffects()
    {
        upgradeEffects = new Dictionary<int, System.Action>
        {
            // Weapon Activation
            { 1000, () => ActivateWeapon(Pistol, pistolStats, 2.5f) },
            { 1001, () => ActivateWeapon(Pistol, pistolStats, 4f) },
            { 1002, () => ActivateWeapon(Pistol, pistolStats, 5.5f) },
            { 1003, () => ActivateWeapon(Pistol, pistolStats, 7f) },
            { 1004, () => ActivateWeapon(Pistol, pistolStats, 12.5f) },
            { 1005, () => ActivateWeapon(Shotgun, shotgunStats, 2f) },
            { 1006, () => ActivateWeapon(Shotgun, shotgunStats, 3f) },
            { 1007, () => ActivateWeapon(Shotgun, shotgunStats, 4f) },
            { 1008, () => ActivateWeapon(Shotgun, shotgunStats, 5f) },
            { 1009, () => ActivateWeapon(Shotgun, shotgunStats, 7.5f) },
            { 1010, () => ActivateWeapon(Rocket, rocketStats, 3f, 1f) },
            { 1011, () => ActivateWeapon(Rocket, rocketStats, 4f, 1.5f) },
            { 1012, () => ActivateWeapon(Rocket, rocketStats, 5f, 2f) },
            { 1013, () => ActivateWeapon(Rocket, rocketStats, 6f, 2.5f) },
            { 1014, () => ActivateWeapon(Rocket, rocketStats, 12f, 5f) },

            // Polymer Tip
            { 0, () => ApplyDamageUpgrade(1.5f) },
            { 1, () => ApplyDamageUpgrade(2.5f) },
            { 2, () => ApplyDamageUpgrade(3.5f) },
            { 3, () => ApplyDamageUpgrade(4.5f) },
            { 4, () => ApplyDamageUpgrade(7.5f) },

            // Gunpowder
            { 5, () => ApplyGunpowderUpgrade(2f, 1f) },
            { 6, () => ApplyGunpowderUpgrade(3.5f, 1f) },
            { 7, () => ApplyGunpowderUpgrade(5f, 1f) },
            { 8, () => ApplyGunpowderUpgrade(6.5f, 1f) },
            { 9, () => ApplyGunpowderUpgrade(10f, 1f) },

            // Extra Magazine
            { 10, () => ApplyFirerateUpgrade(1f) },
            { 11, () => ApplyFirerateUpgrade(1.5f) },
            { 12, () => ApplyFirerateUpgrade(2f) },
            { 13, () => ApplyFirerateUpgrade(2.5f) },
            { 14, () => ApplyFirerateUpgrade(4f) },

            // Steel Plates
            { 15, () => ApplyHealthUpgrade(3) },
            { 16, () => ApplyHealthUpgrade(6) },
            { 17, () => ApplyHealthUpgrade(9) },
            { 18, () => ApplyHealthUpgrade(12) },
            { 19, () => ApplyHealthUpgrade(25) },

            // Turbines
            { 20, () => ApplyMovementSpeedUpgrade(1f) },
            { 21, () => ApplyMovementSpeedUpgrade(1.2f) },
            { 22, () => ApplyMovementSpeedUpgrade(1.4f) },
            { 23, () => ApplyMovementSpeedUpgrade(1.6f) },
            { 24, () => ApplyMovementSpeedUpgrade(2.5f) }
        };
    }

    private void ApplyUpgrade(int upgradeID)
    {
        if (upgradeEffects.ContainsKey(upgradeID))
        {
            upgradeEffects[upgradeID]?.Invoke();
        }
        else
        {
            Debug.LogWarning($"Upgrade ID {upgradeID} not found.");
        }
    }

    private void ActivateWeapon(GameObject weapon, WeaponStats stats, float damageMultiplier, float areaDamageMultiplier = 0)
    {
        weapon.SetActive(true);
        stats.damage += stats.damage / 10 * damageMultiplier;
        if (areaDamageMultiplier > 0)
        {
            stats.areaDamage += stats.areaDamage / 10 * areaDamageMultiplier;
        }
    }

    private void ApplyDamageUpgrade(float multiplier)
    {
        pistolStats.damage += pistolStats.damage / 10 * multiplier;
        shotgunStats.damage += shotgunStats.damage / 10 * multiplier;
        rocketStats.damage += rocketStats.damage / 10 * multiplier;
        rocketStats.areaDamage += rocketStats.areaDamage / 10 * multiplier;
    }

    private void ApplyGunpowderUpgrade(float damageMultiplier, float firerateMultiplier)
    {
        pistolStats.damage += pistolStats.damage / 10 * damageMultiplier;
        pistolStats.firerate -= pistolStats.firerate / 10 * firerateMultiplier;

        shotgunStats.damage += shotgunStats.damage / 10 * damageMultiplier;
        shotgunStats.firerate -= shotgunStats.firerate / 10 * firerateMultiplier;

        rocketStats.damage += rocketStats.damage / 10 * damageMultiplier;
        rocketStats.areaDamage += rocketStats.areaDamage / 10 * damageMultiplier;
        rocketStats.firerate -= rocketStats.firerate / 10 * firerateMultiplier;
    }

    private void ApplyFirerateUpgrade(float multiplier)
    {
        pistolStats.firerate += pistolStats.firerate / 10 * multiplier;
        shotgunStats.firerate += shotgunStats.firerate / 10 * multiplier;
        rocketStats.firerate += rocketStats.firerate / 10 * multiplier;
    }

    private void ApplyHealthUpgrade(int extraHealth)
    {
        playerMaxHealth += extraHealth;
        playerCurrentHealth += extraHealth;
    }

    private void ApplyMovementSpeedUpgrade(float multiplier)
    {
        playerMovementSpeedMultiplier += playerMovementSpeedMultiplier / 10 * multiplier;
    }
}

[System.Serializable]
public class WeaponStats
{
    public float damage;
    public float areaDamage;
    public float firerate;
}