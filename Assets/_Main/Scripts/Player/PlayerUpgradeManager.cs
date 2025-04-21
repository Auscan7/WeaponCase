using UnityEngine;
using System.Collections.Generic;

public class PlayerUpgradeManager : MonoBehaviour
{
    public static PlayerUpgradeManager Instance;

    [Header("Player Stats")]
    [HideInInspector] public float playerCurrentHealth;
    [HideInInspector] public int playerMaxHealth;
    public float playerHealthRegenAmount;
    public float playerLeechAmountPercent;
    public float playerLeechChancePercent;
    public float playerArmor;
    public int playerDodgeChancePercent;
    public float playerMovementSpeedMultiplier;
    public float playerDamageMultiplier;
    public float playerCritDamageMultiplier;
    public float playerCritChancePercent;
    public float playerXpMultiplier;
    public float playerMagnetRadius;

    [Header("Weapon Damage Multipliers")]
    public float pistolDamageMultiplier = 1f;
    public float bowAndArrowDamageMultiplier = 1f;
    public float spearDamageMultiplier = 1f;
    public float blowDartDamageMultiplier = 1f;
    public float slingShotDamageMultiplier = 1f;
    public float flameCircleDamageMultiplier = 1f;
    public float smgDamageMultiplier = 1f;
    public float shotgunDamageMultiplier = 1f;
    public float rocketDamageMultiplier = 1f;
    public float orbitalStrikeDamageMultiplier = 1f;
    public float grenadeDamageMultiplier = 1f;

    [Header("Weapons")]
    public GameObject pistol;
    public GameObject bowAndArrow;
    public GameObject spear;
    public GameObject blowDart;
    public GameObject slingShot;
    public GameObject flameCircle;
    public GameObject smg;
    public GameObject shotgun;
    public GameObject rocket;
    public GameObject orbitalStrike;
    public GameObject grenade;

    [Header("Weapon Stats")]
    public WeaponStats pistolStats;
    public WeaponStats bowAndArrowStats;
    public WeaponStats spearStats;
    public WeaponStats blowDartStats;
    public WeaponStats slingShotStats;
    public WeaponStats flameCircleStats;
    public WeaponStats smgStats;
    public WeaponStats shotgunStats;
    public WeaponStats rocketStats;
    public WeaponStats orbitalStrikeStats;
    public WeaponStats grenadeStats;

    // Store the base damage values for recalculation
    [Header("Base Weapon Damage")]
    public float basePistolDamage = 10f;
    public float baseBowAndArrowDamage = 15f;
    public float baseSpearDamage = 15f;
    public float baseBlowDartDamage = 5f;
    public float baseSlingShotDamage = 18f;
    public float baseFlameCircleDamage = 5f;
    public float baseSlingShotAreaDamage = 6f;
    public float baseSMGDamage = 2f;
    public float baseShotgunDamage = 6f;
    public float baseRocketDamage = 24f;
    public float baseRocketAreaDamage = 8f;
    public float baseOrbitalStrikeDamage = 30f;
    public float baseGrenadeDamage = 30f;

    [Header("Base Weapon Projectile Count")]
    public int baseShotgunProjectileCount = 3;
    public int baseGrenadeProjectileCount = 3;

    [Header("Boats")]
    public GameObject Default;
    public GameObject Second;
    //public GameObject Third;

    [Header("Boat Stats")]
    private Dictionary<string, BoatStats> boatModifiers;

    public string SelectedBoatName { get; private set; }

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

        //pull player stats from the permenant upgrade manager
        playerMaxHealth = PermenantUpgradeManager.Instance.maxHealth;
        playerArmor = PermenantUpgradeManager.Instance.armor;
        playerDamageMultiplier = PermenantUpgradeManager.Instance.damage;
        playerMovementSpeedMultiplier = PermenantUpgradeManager.Instance.moveSpeed;

        InitializeBoatModifiers();
        ResetWeaponDamage();
    }

    private void Start()
    {
        InitializeWeaponDictionary();
        // Optionally set a default active weapon (if needed)
        //ActivateWeapon("BowAndArrow");
    }

    private void InitializeBoatModifiers()
    {
        // healthMultiplier, armorMultiplier, damageMultiplier, speedMultiplier
        boatModifiers = new Dictionary<string, BoatStats>
        {
            { "Wooden Boat", new BoatStats(1f, 1f, 1f, 1f) },
            { "Battle Ship", new BoatStats(1.25f, 1.25f, 0.75f, 0.75f) },
            //{ "Third Boat", new BoatStats(0.5f, 0.5f, 1.5f, 1.2f) }
        };
    }

    private void InitializeWeaponDictionary()
    {
        // Create a dictionary to store your weapons for easy reference.
        weaponDict = new Dictionary<string, GameObject>
        {
            { "Pistol", pistol },
            { "BowAndArrow", bowAndArrow },
            { "Spear", spear },
            { "BlowDart", blowDart },
            { "SlingShot", slingShot },
            { "FlameCircle", flameCircle },
            { "SMG", smg },
            { "Shotgun", shotgun },
            { "Rocket", rocket },
            { "OrbitalStrike", orbitalStrike },
            { "Grenade", grenade }
        };
    }

    /// <summary>
    /// Activates the weapon with the specified key without deactivating others.
    /// </summary>
    public void ActivateWeapon(string weaponKey)
    {
        if (weaponDict.ContainsKey(weaponKey))
        {
            var weapon = weaponDict[weaponKey];
            weaponDict[weaponKey].SetActive(true);

            // Fire the event
            GameEvents.WeaponActivated(weapon);
            // Attach the weapon to the cooldown UI manager
            WeaponCooldownUIManager.Instance.AttachWeapon(weaponKey, weapon.GetComponent<Weapon>().weaponIcon);

            // Refresh the weapon list in the firing script
            EnemyDetection firingScript = FindAnyObjectByType<EnemyDetection>();
            if (firingScript != null)
            {
                firingScript.Start(); // Reinitialize weapon list
            }
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


    // Call this method to apply the boat stats to the player.
    public void ApplyBoatStats(string boatName)
    {
        SelectedBoatName = boatName;

        if (boatModifiers.ContainsKey(boatName))
        {
            BoatStats stats = boatModifiers[boatName];
            playerMaxHealth = Mathf.RoundToInt(playerMaxHealth * stats.healthMultiplier);
            playerCurrentHealth = playerMaxHealth;
            playerArmor *= stats.armorMultiplier;
            playerDamageMultiplier *= stats.damageMultiplier;
            playerMovementSpeedMultiplier *= stats.speedMultiplier;

            // Recalculate weapon damage based on the new playerDamageMultiplier
            UpdateWeaponDamage();
        }
        else
        {
            Debug.LogWarning($"Boat {boatName} not found!");
        }
    }

    // Call this method to reset the player stats to their base values.
    public void ResetBoatStats()
    {
        playerMaxHealth = PermenantUpgradeManager.Instance.maxHealth;
        playerCurrentHealth = playerMaxHealth;
        playerArmor = PermenantUpgradeManager.Instance.armor;
        playerDamageMultiplier = PermenantUpgradeManager.Instance.damage;
        playerMovementSpeedMultiplier = PermenantUpgradeManager.Instance.moveSpeed;

        ResetWeaponDamage();
    }

    /// <summary>
    /// Resets the weapon damage to their base values (using playerDamageMultiplier)
    /// </summary>
    public void ResetWeaponDamage()
    {
        pistolStats.damage = basePistolDamage * playerDamageMultiplier;
        bowAndArrowStats.damage = baseBowAndArrowDamage * playerDamageMultiplier;
        spearStats.damage = baseSpearDamage * playerDamageMultiplier;
        blowDartStats.damage = baseBlowDartDamage * playerDamageMultiplier;
        slingShotStats.damage = baseSlingShotDamage * playerDamageMultiplier;
        slingShotStats.areaDamage = baseSlingShotAreaDamage * playerDamageMultiplier;
        flameCircleStats.damage = baseFlameCircleDamage * playerDamageMultiplier;
        smgStats.damage = baseSMGDamage * playerDamageMultiplier;
        shotgunStats.damage = baseShotgunDamage * playerDamageMultiplier;
        rocketStats.damage = baseRocketDamage * playerDamageMultiplier;
        rocketStats.areaDamage = baseRocketAreaDamage * playerDamageMultiplier;
        orbitalStrikeStats.damage = baseOrbitalStrikeDamage * playerDamageMultiplier;
        grenadeStats.damage = baseGrenadeDamage * playerDamageMultiplier;
    }

    /// <summary>
    /// Call this method after any upgrade is applied that affects damage.
    /// </summary>
    public void UpdateWeaponDamage()
    {
        // Apply both player-wide and weapon-specific multipliers
        pistolStats.damage = basePistolDamage * playerDamageMultiplier * pistolDamageMultiplier;
        bowAndArrowStats.damage = baseBowAndArrowDamage * playerDamageMultiplier * bowAndArrowDamageMultiplier;
        spearStats.damage = baseSpearDamage * playerDamageMultiplier * spearDamageMultiplier;
        blowDartStats.damage = baseBlowDartDamage * playerDamageMultiplier * blowDartDamageMultiplier;
        slingShotStats.damage = baseSlingShotDamage * playerDamageMultiplier * slingShotDamageMultiplier;
        slingShotStats.areaDamage = baseSlingShotAreaDamage * playerDamageMultiplier * slingShotDamageMultiplier;
        flameCircleStats.damage = baseFlameCircleDamage * playerDamageMultiplier * flameCircleDamageMultiplier;
        smgStats.damage = baseSMGDamage * playerDamageMultiplier * smgDamageMultiplier;
        shotgunStats.damage = baseShotgunDamage * playerDamageMultiplier * shotgunDamageMultiplier;
        rocketStats.damage = baseRocketDamage * playerDamageMultiplier * rocketDamageMultiplier;
        rocketStats.areaDamage = baseRocketAreaDamage * playerDamageMultiplier * rocketDamageMultiplier;
        orbitalStrikeStats.damage = baseOrbitalStrikeDamage * playerDamageMultiplier * orbitalStrikeDamageMultiplier;
        grenadeStats.damage = baseGrenadeDamage * playerDamageMultiplier * grenadeDamageMultiplier;
    }

}

[System.Serializable]
public class BoatStats
{
    public float healthMultiplier;
    public float armorMultiplier;
    public float damageMultiplier;
    public float speedMultiplier;

    public BoatStats(float healthMult, float armorMult, float damageMult, float speedMult)
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
    public float poisonDamage;
    public float areaDamage;
    public float firerate;
    public float range;
}
