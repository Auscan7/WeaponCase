using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerStatManager : CharacterStatManager
{
    [SerializeField] private float damageCooldown = 0.15f;
    private bool isOnCooldown = false;
    public TMP_Text HPText;

    public GameObject DeadScreen;

    protected override void Start()
    {
        UpgradeManager.Instance.playerCurrentHealth = UpgradeManager.Instance.playerMaxHealth;
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

    private IEnumerator DamageCooldownCoroutine()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(damageCooldown);
        isOnCooldown = false;
    }
}