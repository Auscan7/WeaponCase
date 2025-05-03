using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatManager : CharacterStatManager
{
    private float damageCooldown = 0.35f;
    private float regenCooldown = 1f; // Time between ticks
    private bool isOnCooldown = false;
    public TMP_Text HPText;

    [Header("UI Buff Indicators")]
    [SerializeField] private GameObject damageBoostIcon;
    [SerializeField] private Image damageBoostOverlay;

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

    public override void TakeDamage(float damage, Color color = default)
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

        float reducedDamage = damage / (1 + ((PlayerUpgradeManager.Instance.playerArmor * 7.5f) / 100));
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
        PlayerInputManager.instance.enabled = false;
        StartCoroutine(PlayDeathAnimation());
    }

    private IEnumerator PlayDeathAnimation()
    {
        float duration = 0.8f;
        float elapsed = 0f;

        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.zero;
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            transform.localScale = Vector3.Lerp(startScale, endScale, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        GameTimeManager.Instance.LevelFailedTrigger();
        PlayerInputManager.instance.enabled = true;
        Destroy(gameObject);
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
                StartDamageBoost(20f);
                AudioManager.instance.PlaySoundSFX(AudioManager.instance.wrenchPickUpSFX);
                break;

            case "Pearl":
                HandlePearlPickup();
                AudioManager.instance.PlaySoundSFX(AudioManager.instance.wrenchPickUpSFX);
                break;

            default:
                return; // Exit if it's not a relevant tag
        }

        StartCoroutine(PlayPickupAnimationAndDestroy(collision.gameObject));
    }

    private IEnumerator PlayPickupAnimationAndDestroy(GameObject pickup)
    {
        if (pickup == null) yield break;

        // Optional: disable collider immediately
        Collider2D col = pickup.GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        float duration = 0.3f;
        float elapsed = 0f;

        Vector3 startScale = pickup.transform.localScale;
        Vector3 endScale = Vector3.zero;

        Vector3 startPosition = pickup.transform.position;
        Vector3 endPosition = startPosition + new Vector3(0, 0.5f, 0); // float up slightly

        SpriteRenderer sr = pickup.GetComponent<SpriteRenderer>();
        Color startColor = sr != null ? sr.color : Color.white;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f); // fade out

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            if (pickup != null)
            {
                pickup.transform.localScale = Vector3.Lerp(startScale, endScale, t);
                pickup.transform.position = Vector3.Lerp(startPosition, endPosition, t);
                if (sr != null)
                    sr.color = Color.Lerp(startColor, endColor, t);
            }

            if (sr != null)
                sr.color = Color.Lerp(startColor, endColor, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        if (pickup != null)
        {
            Destroy(pickup);
        }
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
        CurrencyManager.Instance.AddCurrency(10);
    }

    //Magnet
    private IEnumerator IncreaseMagnetRadiusTemporarily()
    {
        PlayerUpgradeManager.Instance.playerMagnetRadius *= 100f;

        yield return new WaitForSeconds(0.5f);

        PlayerUpgradeManager.Instance.playerMagnetRadius /= 100f;
    }

    //Damage
    private Coroutine damageBoostCoroutine;
    private IEnumerator IncreaseDamageTemporarilyRoutine(float duration)
    {
        damageBoostIcon.SetActive(true);
        damageBoostOverlay.fillAmount = 1f;

        PlayerUpgradeManager.Instance.playerDamageMultiplier *= 2f;
        PlayerUpgradeManager.Instance.UpdateWeaponDamage();

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            damageBoostOverlay.fillAmount = Mathf.Clamp01(1f - (elapsed / duration));
            yield return null;
        }

        PlayerUpgradeManager.Instance.playerDamageMultiplier /= 2f;
        PlayerUpgradeManager.Instance.UpdateWeaponDamage();

        damageBoostIcon.SetActive(false);
        damageBoostCoroutine = null;
    }

    private void StartDamageBoost(float duration)
    {
        if (damageBoostCoroutine != null)
        {
            StopCoroutine(damageBoostCoroutine);
            PlayerUpgradeManager.Instance.playerDamageMultiplier /= 2f;
            PlayerUpgradeManager.Instance.UpdateWeaponDamage();
            damageBoostCoroutine = null;
        }

        damageBoostCoroutine = StartCoroutine(IncreaseDamageTemporarilyRoutine(duration));
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
