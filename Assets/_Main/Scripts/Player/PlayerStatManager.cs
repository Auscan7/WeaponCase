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
        PlayerUpgradeManager.Instance.playerCurrentHealth = PlayerUpgradeManager.Instance.playerMaxHealth;
        StartCoroutine(HealthRegenCoroutine()); // Start regen system
    }

    protected override void Update()
    {
        if (Mathf.RoundToInt(PlayerUpgradeManager.Instance.playerCurrentHealth) <= 0)
        {
            HandleDeath();
        }

        if (HPText != null)
        {
            HPText.text = "HP: " + Mathf.RoundToInt(PlayerUpgradeManager.Instance.playerCurrentHealth);
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
        if (dodgeRoll < PlayerUpgradeManager.Instance.playerDodgeChancePercent)
        {
            FloatingTextManager.Instance.ShowFloatingText(transform.position, "MISS!", Color.cyan, 1.25f, 0.2f, 0.5f);
            StartCoroutine(DamageCooldownCoroutine());
            return; // No damage applied
        }

        float reducedDamage = damage / (1 + ((PlayerUpgradeManager.Instance.playerArmor * 10) / 100));
        PlayerUpgradeManager.Instance.playerCurrentHealth = Mathf.Max(0, Mathf.Round(PlayerUpgradeManager.Instance.playerCurrentHealth - reducedDamage));

        UpdateHealthBar();
        FloatingTextManager.Instance.ShowFloatingText(transform.position, reducedDamage.ToString("F0"), Color.red, 1.35f, 0.25f, 0.65f);
        StartCoroutine(DamageCooldownCoroutine());
    }

    // Magnet radius calculation at runtime
    private void UpdateMagnetRadius()
    {   
        if (magnetCollider != null)
        {
            magnetCollider.radius = PlayerUpgradeManager.Instance.playerMagnetRadius;
        }
    }

    // UI helth bar
    private void UpdateHealthBar()
    {
        healthBar.fillAmount = Mathf.Clamp01(PlayerUpgradeManager.Instance.playerCurrentHealth / PlayerUpgradeManager.Instance.playerMaxHealth);
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

            case "Pearl":
                HandlePearlPickup();
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
        float regenAmount = Mathf.RoundToInt((PlayerUpgradeManager.Instance.playerMaxHealth / 10) * 2f);
        PlayerUpgradeManager.Instance.playerCurrentHealth = Mathf.Min(
            PlayerUpgradeManager.Instance.playerCurrentHealth + regenAmount,
            PlayerUpgradeManager.Instance.playerMaxHealth
        );

        FloatingTextManager.Instance.ShowFloatingText(transform.position, regenAmount.ToString("0.#"), Color.green, 1.55f, 0.3f, 0.75f);
    }

    //Pearl
    private void HandlePearlPickup()
    {
        CurrencyManager.Instance.AddCurrency(1);
    }

    //Magnet
    private IEnumerator IncreaseMagnetRadiusTemporarily()
    {
        PlayerUpgradeManager.Instance.playerMagnetRadius *= 100f;

        yield return new WaitForSeconds(5f);

        PlayerUpgradeManager.Instance.playerMagnetRadius /= 100f;
    }

    //Damage
    private IEnumerator IncreaseDamageTemporarily()
    {
        PlayerUpgradeManager.Instance.playerDamageMultiplier *= 2f;
        PlayerUpgradeManager.Instance.UpdateWeaponDamage();

        yield return new WaitForSeconds(20f);

        PlayerUpgradeManager.Instance.playerDamageMultiplier /= 2f;
        PlayerUpgradeManager.Instance.UpdateWeaponDamage();
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

            if (PlayerUpgradeManager.Instance.playerCurrentHealth < PlayerUpgradeManager.Instance.playerMaxHealth)
            {
                PlayerUpgradeManager.Instance.playerCurrentHealth = Mathf.Min(
                    PlayerUpgradeManager.Instance.playerMaxHealth,
                    PlayerUpgradeManager.Instance.playerCurrentHealth + PlayerUpgradeManager.Instance.playerHealthRegenAmount
                );
                //FloatingTextManager.Instance.ShowFloatingText(transform.position, UpgradeManager.Instance.playerHealthRegenAmount.ToString("0.#"), Color.green, 1.2f, 0.6f, 0.75f);
                UpdateHealthBar();
            }
        }
    }
}
