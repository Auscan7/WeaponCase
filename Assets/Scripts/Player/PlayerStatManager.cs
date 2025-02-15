using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerStatManager : CharacterStatManager
{
    [SerializeField] private float damageCooldown = 0.3f;
    [SerializeField] private float regenCooldown = 3f; // Time between ticks
    private bool isOnCooldown = false;
    public TMP_Text HPText;
    public GameObject DeadScreen;

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
    }

    public override void TakeDamage(float damage)
    {
        if (isOnCooldown) return;

        // Generate a random value between 0 and 100
        int dodgeRoll = Random.Range(0, 101); // 0 to 100 inclusive

        // If the roll is less than the dodge chance, dodge the attack
        if (dodgeRoll < UpgradeManager.Instance.playerDodgeChancePercent)
        {
            ShowFloatingDodgeText();
            StartCoroutine(DamageCooldownCoroutine());
            return; // No damage applied
        }

        float reducedDamage = damage / (1 + ((UpgradeManager.Instance.playerArmor * 10) / 100));
        UpgradeManager.Instance.playerCurrentHealth = Mathf.Max(0, Mathf.Round(UpgradeManager.Instance.playerCurrentHealth - reducedDamage));

        UpdateHealthBar();
        ShowFloatingDamage(reducedDamage);
        StartCoroutine(DamageCooldownCoroutine());
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

    protected override void ShowFloatingDamage(float damage)
    {
        if (floatingDamagePrefab != null)
        {
            Vector3 randomOffset = new Vector3(Random.Range(xOffsetRange.x, xOffsetRange.y), Random.Range(yOffsetRange.x, yOffsetRange.y), 0);

            GameObject floatingText = Instantiate(floatingDamagePrefab, transform.position + Vector3.up + randomOffset, Quaternion.identity);
            TMP_Text textComponent = floatingText.GetComponentInChildren<TMP_Text>();

            if (textComponent != null)
            {
                textComponent.text = damage.ToString("F0");
                textComponent.color = Color.red;
            }
        }
        else
        {
            Debug.LogWarning("Floating Damage Prefab is not assigned in the inspector.");
        }
    }

    private void ShowFloatingRegenText(float regen)
    {
        if (floatingDamagePrefab != null)
        {
            Vector3 randomOffset = new Vector3(Random.Range(xOffsetRange.x, xOffsetRange.y), Random.Range(yOffsetRange.x, yOffsetRange.y), 0);

            GameObject floatingText = Instantiate(floatingDamagePrefab, transform.position + Vector3.up + randomOffset, Quaternion.identity);
            TMP_Text textComponent = floatingText.GetComponentInChildren<TMP_Text>();

            if (textComponent != null)
            {
                textComponent.text = regen.ToString("0.#");
                textComponent.color = Color.green;
            }
        }
        else
        {
            Debug.LogWarning("Floating Regen Prefab is not assigned in the inspector.");
        }
    }

    private void ShowFloatingDodgeText()
    {
        if (floatingDamagePrefab != null)
        {
            Vector3 randomOffset = new Vector3(Random.Range(xOffsetRange.x, xOffsetRange.y), Random.Range(yOffsetRange.x, yOffsetRange.y), 0);
            GameObject floatingText = Instantiate(floatingDamagePrefab, transform.position + Vector3.up + randomOffset, Quaternion.identity);
            TMP_Text textComponent = floatingText.GetComponentInChildren<TMP_Text>();

            if (textComponent != null)
            {
                textComponent.text = "DODGED!";
                textComponent.color = Color.yellow;
            }
        }
        else
        {
            Debug.LogWarning("Floating Dodge Prefab is not assigned in the inspector.");
        }
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
            ShowFloatingRegenText(regenAmount);
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
                ShowFloatingRegenText(UpgradeManager.Instance.playerHealthRegenAmount);
                UpdateHealthBar();
            }
        }
    }
}
