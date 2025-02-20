using UnityEngine;
using System.Collections.Generic;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    [Header("Player Stats")]
    public float playerCurrentHealth;
    public int playerMaxHealth;
    public float playerHealthRegenAmount = 1f;
    public float playerArmor;
    public int playerDodgeChancePercent = 20;
    public float playerMovementSpeedMultiplier = 1f;
    public float playerDamageMultiplier = 1f;
    public float playerCritDamageMultiplier = 5f;
    public float playerCritChancePercent = 10f;
    public float playerXpMultiplier = 1f;
    public float playerMagnetRadius = 1.75f;

    [Header("Weapon Damage Multipliers")]
    public float pistolDamageMultiplier = 1f;
    public float shotgunDamageMultiplier = 1f;
    public float rocketDamageMultiplier = 1f;

    [Header("Weapons")]
    public GameObject pistol;
    public GameObject shotgun;
    public GameObject rocket;

    [Header("Weapon Stats")]
    public WeaponStats pistolStats;
    public WeaponStats shotgunStats;
    public WeaponStats rocketStats;

    // Store the base damage values for recalculation
    [Header("Base Weapon Damage")]
    public float basePistolDamage = 10f;
    public float baseShotgunDamage = 6f;
    public float baseRocketDamage = 20f;
    public float baseRocketAreaDamage = 8f;

    [Header("Base Weapon Projectile Count")]
    public int baseShotgunProjectileCount = 3;

    [Header("Cases")]
    public GameObject Default;
    public GameObject Second;
    public GameObject Third;

    [Header("Case Stats")]
    private Dictionary<string, CaseStats> caseModifiers;

    // Dictionary to manage weapon references easily.
    private Dictionary<string, GameObject> weaponDict;

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

        InitializeCaseModifiers();
        ResetWeaponDamage();
        InitializeWeaponDictionary();

        // Optionally set a default active weapon (if needed)
        ActivateWeapon("Pistol");
    }

    private void InitializeCaseModifiers()
    {
        caseModifiers = new Dictionary<string, CaseStats>
        {
            { "Default Case", new CaseStats(1.0f, 1.0f, 1.0f, 1.0f) },
            { "Second Case", new CaseStats(0.8f, 0.8f, 1.2f, 1.0f) },
            { "Third Case", new CaseStats(0.5f, 0.5f, 1.5f, 1.2f) }
        };
    }

    private void InitializeWeaponDictionary()
    {
        // Create a dictionary to store your weapons for easy reference.
        weaponDict = new Dictionary<string, GameObject>
        {
            { "Pistol", pistol },
            { "Shotgun", shotgun },
            { "Rocket", rocket }
        };
    }

    /// <summary>
    /// Activates the weapon with the specified key without deactivating others.
    /// </summary>
    public void ActivateWeapon(string weaponKey)
    {
        if (weaponDict.ContainsKey(weaponKey))
        {
            weaponDict[weaponKey].SetActive(true);
            Debug.Log("Activated weapon: " + weaponKey);
        }
        else
        {
            Debug.LogWarning("Weapon key not found: " + weaponKey);
        }
    }

    public bool IsWeaponActive(string weaponKey)
    {
        if (weaponDict.TryGetValue(weaponKey, out GameObject weapon))
        {
            return weapon.activeInHierarchy;
        }
        Debug.LogWarning("Weapon key not found: " + weaponKey);
        return false;
    }


    public void SetStartingWeapon(GameObject selectedWeapon)
    {
        // Deactivate all weapons first.
        foreach (var weapons in weaponDict)
        {
            weapons.Value.SetActive(false);
        }
        // Activate the selected starting weapon.
        selectedWeapon.SetActive(true);
    }


    public void ApplyCaseStats(string caseName)
    {
        if (caseModifiers.TryGetValue(caseName, out CaseStats caseStats))
        {
            playerMaxHealth = Mathf.RoundToInt(playerMaxHealth * caseStats.healthMultiplier);
            playerCurrentHealth = playerMaxHealth;
            playerArmor *= caseStats.armorMultiplier;
            playerDamageMultiplier = caseStats.damageMultiplier;
            playerMovementSpeedMultiplier *= caseStats.speedMultiplier;

            // Recalculate weapon damage based on the new playerDamageMultiplier
            UpdateWeaponDamage();
        }
        else
        {
            Debug.LogWarning($"Case {caseName} not found!");
        }
    }

    public void ResetCaseStats()
    {
        playerMaxHealth = 100;
        playerCurrentHealth = playerMaxHealth;
        playerArmor = 10;
        playerDamageMultiplier = 1f;
        playerMovementSpeedMultiplier = 1f;

        ResetWeaponDamage();
    }

    /// <summary>
    /// Resets the weapon damage to their base values (using playerDamageMultiplier)
    /// </summary>
    public void ResetWeaponDamage()
    {
        pistolStats.damage = basePistolDamage * playerDamageMultiplier;
        shotgunStats.damage = baseShotgunDamage * playerDamageMultiplier;
        rocketStats.damage = baseRocketDamage * playerDamageMultiplier;
        rocketStats.areaDamage = baseRocketAreaDamage * playerDamageMultiplier;
    }

    /// <summary>
    /// Call this method after any upgrade is applied that affects damage.
    /// </summary>
    public void UpdateWeaponDamage()
    {
        // Apply both player-wide and weapon-specific multipliers
        pistolStats.damage = basePistolDamage * playerDamageMultiplier * pistolDamageMultiplier;
        shotgunStats.damage = baseShotgunDamage * playerDamageMultiplier * shotgunDamageMultiplier;
        rocketStats.damage = baseRocketDamage * playerDamageMultiplier * rocketDamageMultiplier;
        rocketStats.areaDamage = baseRocketAreaDamage * playerDamageMultiplier * rocketDamageMultiplier;
    }

}

[System.Serializable]
public class CaseStats
{
    public float healthMultiplier;
    public float armorMultiplier;
    public float damageMultiplier;
    public float speedMultiplier;

    public CaseStats(float healthMult, float armorMult, float damageMult, float speedMult)
    {
        healthMultiplier = healthMult;
        armorMultiplier = armorMult;
        damageMultiplier = damageMult;
        speedMultiplier = speedMult;
    }
}

[System.Serializable]
public class WeaponStats
{
    public float damage;
    public float areaDamage;
    public float firerate;
}
