using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerStatManager : CharacterStatManager
{
    [SerializeField] private float damageCooldown = 0.15f;
    private bool isOnCooldown = false;
    public TMP_Text HPText;

    protected override void Start()
    {
        UpgradeManager.Instance.playercurrentHealth = UpgradeManager.Instance.playerMaxHealth;
    }

    protected override void Update()
    {
        if (UpgradeManager.Instance.playercurrentHealth <= 0)
        {
            HandleDeath();
        }

        if (HPText != null)
        {
            HPText.text = "HP: " + UpgradeManager.Instance.playercurrentHealth.ToString();
        }
    }

    private void FixedUpdate()
    {
        UpdateHealthBar();
    }

    public override void TakeDamage(float damage)
    {
        if (isOnCooldown) return;

        UpgradeManager.Instance.playercurrentHealth -= damage;
        UpdateHealthBar();

        ShowFloatingDamage(damage);
        StartCoroutine(DamageCooldownCoroutine());
    }

    private void UpdateHealthBar()
    {
        healthBar.fillAmount = UpgradeManager.Instance.playercurrentHealth / UpgradeManager.Instance.playerMaxHealth;
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
