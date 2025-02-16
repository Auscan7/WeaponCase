using DG.Tweening;
using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class PlayerStatManager : CharacterStatManager
{
    [SerializeField] private float damageCooldown = 0.3f;
    [SerializeField] private float regenCooldown = 3f; // Time between ticks
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

    private void UpdateMagnetRadius()
    {   
        if (magnetCollider != null)
        {
            magnetCollider.radius = UpgradeManager.Instance.playerMagnetRadius;
        }
    }

    private void UpdateHealthBar()
    {
        healthBar.fillAmount = Mathf.Clamp01(UpgradeManager.Instance.playerCurrentHealth / UpgradeManager.Instance.playerMaxHealth);
    }

    public override void HandleDeath()
    {
        DeadScreen.SetActive(true);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wrench")
        {
            AudioManager.instance.PlaySoundSFX(AudioManager.instance.wrenchPickUpSFX);

            float regenAmount = Mathf.RoundToInt((UpgradeManager.Instance.playerMaxHealth / 10) * 1.5f);
            UpgradeManager.Instance.playerCurrentHealth += regenAmount;

            if (UpgradeManager.Instance.playerCurrentHealth > UpgradeManager.Instance.playerMaxHealth)
            {
                UpgradeManager.Instance.playerCurrentHealth = UpgradeManager.Instance.playerMaxHealth;
            }
            FloatingTextManager.Instance.ShowFloatingText(transform.position, regenAmount.ToString("0.#"), Color.green, 1.45f, 0.3f, 0.75f);
            Destroy(collision.gameObject); // Destroy the wrench
        }
    }

    private IEnumerator DamageCooldownCoroutine()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(damageCooldown);
        isOnCooldown = false;
    }

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
                FloatingTextManager.Instance.ShowFloatingText(transform.position, UpgradeManager.Instance.playerHealthRegenAmount.ToString("0.#"), Color.green, 1.2f, 0.6f, 0.75f);
                UpdateHealthBar();
            }
        }
    }
}
