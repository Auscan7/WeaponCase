using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerStatManager : CharacterStatManager
{
    private float damageCooldown = 0.35f;
    private float regenCooldown = 3f; // Time between ticks
    private bool isOnCooldown = false;
    public TMP_Text HPText;
    public GameObject DeadScreen;

    [Header("Magnet")]
    [SerializeField] private CircleCollider2D magnetCollider;

    protected override void Start()
    {
        UpgradeManager.Instance.playerCurrentHealth = UpgradeManager.Instance.playerMaxHealth;
        StartCoroutine(HealthRegenCoroutine()); // Start regen system
    }

    protected override void Update()
    {
        if (Mathf.RoundToInt(UpgradeManager.Instance.playerCurrentHealth) <= 0)
        {
            HandleDeath();
        }

        if (HPText != null)
        {
            HPText.text = "HP: " + Mathf.RoundToInt(UpgradeManager.Instance.playerCurrentHealth);
        }
    }

    private void FixedUpdate()
    {
        UpdateHealthBar();
        UpdateMagnetRadius();
    }

    public override void TakeDamage(float damage)
    {
        if (isOnCooldown) return;

        // Generate a random value between 0 and 100
        int dodgeRoll = Random.Range(0, 101); // 0 to 100 inclusive

        // If the roll is less than the dodge chance, dodge the attack
        if (dodgeRoll < UpgradeManager.Instance.playerDodgeChancePercent)
        {
            FloatingTextManager.Instance.ShowFloatingText(transform.position, "MISS!", Color.cyan, 1.25f, 0.2f, 0.5f);
            StartCoroutine(DamageCooldownCoroutine());
            return; // No damage applied
        }

        float reducedDamage = damage / (1 + ((UpgradeManager.Instance.playerArmor * 10) / 100));
        UpgradeManager.Instance.playerCurrentHealth = Mathf.Max(0, Mathf.Round(UpgradeManager.Instance.playerCurrentHealth - reducedDamage));

        UpdateHealthBar();
        FloatingTextManager.Instance.ShowFloatingText(transform.position, damage.ToString("F0"), Color.red, 1.35f, 0.25f, 0.65f);
        StartCoroutine(DamageCooldownCoroutine());
    }

    // Magnet radius calculation at runtime
    private void UpdateMagnetRadius()
    {   
        if (magnetCollider != null)
        {
            magnetCollider.radius = UpgradeManager.Instance.playerMagnetRadius;
        }
    }

    // UI helth bar
    private void UpdateHealthBar()
    {
        healthBar.fillAmount = Mathf.Clamp01(UpgradeManager.Instance.playerCurrentHealth / UpgradeManager.Instance.playerMaxHealth);
    }

    public override void HandleDeath()
    {
        DeadScreen.SetActive(true);
        Destroy(gameObject); // make this fancier later
    }

    // pickup system
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;

        switch (tag)
        {
            case "Wrench":
                HandleWrenchPickup();
                AudioManager.instance.PlaySoundSFX(AudioManager.instance.wrenchPickUpSFX);
                break;

            case "Magnet":
                StartCoroutine(IncreaseMagnetRadiusTemporarily());
                AudioManager.instance.PlaySoundSFX(AudioManager.instance.wrenchPickUpSFX);
                break;

            case "Damage":
                StartCoroutine(IncreaseDamageTemporarily());
                AudioManager.instance.PlaySoundSFX(AudioManager.instance.wrenchPickUpSFX);
                break;

            default:
                return; // Exit if it's not a relevant tag
        }

        // Destroy the pickup object
        Destroy(collision.gameObject);
    }

    //Wrench
    private void HandleWrenchPickup()
    {
        float regenAmount = Mathf.RoundToInt((UpgradeManager.Instance.playerMaxHealth / 10) * 2f);
        UpgradeManager.Instance.playerCurrentHealth = Mathf.Min(
            UpgradeManager.Instance.playerCurrentHealth + regenAmount,
            UpgradeManager.Instance.playerMaxHealth
        );

        FloatingTextManager.Instance.ShowFloatingText(transform.position, regenAmount.ToString("0.#"), Color.green, 1.55f, 0.3f, 0.75f);
    }

    //Magnet
    private IEnumerator IncreaseMagnetRadiusTemporarily()
    {
        UpgradeManager.Instance.playerMagnetRadius *= 100f;

        yield return new WaitForSeconds(5f);

        UpgradeManager.Instance.playerMagnetRadius /= 100f;
    }

    //Damage
    private IEnumerator IncreaseDamageTemporarily()
    {
        UpgradeManager.Instance.playerDamageMultiplier *= 2f;
        UpgradeManager.Instance.UpdateWeaponDamage();

        yield return new WaitForSeconds(20f);

        UpgradeManager.Instance.playerDamageMultiplier /= 2f;
        UpgradeManager.Instance.UpdateWeaponDamage();
    }

    // if on cooldown prevent taking damage
    private IEnumerator DamageCooldownCoroutine()
    {
        isOnCooldown = true;

        yield return new WaitForSeconds(damageCooldown);

        isOnCooldown = false;
    }

    // passively regen health
    private IEnumerator HealthRegenCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(regenCooldown);

            if (UpgradeManager.Instance.playerCurrentHealth < UpgradeManager.Instance.playerMaxHealth)
            {
                UpgradeManager.Instance.playerCurrentHealth = Mathf.Min(
                    UpgradeManager.Instance.playerMaxHealth,
                    UpgradeManager.Instance.playerCurrentHealth + UpgradeManager.Instance.playerHealthRegenAmount
                );
                //FloatingTextManager.Instance.ShowFloatingText(transform.position, UpgradeManager.Instance.playerHealthRegenAmount.ToString("0.#"), Color.green, 1.2f, 0.6f, 0.75f);
                UpdateHealthBar();
            }
        }
    }
}
