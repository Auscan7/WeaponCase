using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;


public class CharacterStatManager : MonoBehaviour
{
    CharacterManager character;

    [Header("Health Bar")]
    [SerializeField] public float currentHealth;
    [SerializeField] public int maxHealth;
    [SerializeField] public Image healthBar;
    [SerializeField] public GameObject healthBarParent;

    [Header("Floating Damage")]
    [SerializeField] public GameObject floatingDamagePrefab;

    [Header("Floating Text Offset")]
    [SerializeField] public Vector2 xOffsetRange = new Vector2(-0.5f, 0.5f);
    [SerializeField] public Vector2 yOffsetRange = new Vector2(0.5f, 1f);


    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    protected virtual void Update()
    {
        if (currentHealth <= 0)
        {
            HandleDeath();
        }
    }

    public virtual void TakeDamage(float damage)
    {
        // Generate a random value between 0 and 100
        int critChance = Random.Range(0, 101); // 0 to 100 inclusive

        // If the roll is less than the crit chance, deal crit damage
        if (critChance < UpgradeManager.Instance.playerCritChancePercent)
        {    
            float critDamage = (damage += damage / 10 * UpgradeManager.Instance.playerCritDamageMultiplier);
            currentHealth -= critDamage;
            ShowFloatingCritDamageText(critDamage);
            return; // No crit damage applied
        }

        currentHealth -= damage;

        healthBar.fillAmount = currentHealth / maxHealth;

        ShowFloatingDamage(damage);
    }

    public virtual void HandleDeath()
    {
        Destroy(gameObject);
    }

    protected virtual void ShowFloatingDamage(float damage)
    {
        if (floatingDamagePrefab != null)
        {
            Vector3 randomOffset = new Vector3(Random.Range(xOffsetRange.x, xOffsetRange.y),Random.Range(yOffsetRange.x, yOffsetRange.y),0);

            GameObject floatingText = Instantiate(floatingDamagePrefab, transform.position + Vector3.up + randomOffset, Quaternion.identity);
            TMP_Text textComponent = floatingText.GetComponentInChildren<TMP_Text>();

            if (textComponent != null)
            {
                textComponent.text = damage.ToString("F0");
                // Optional: Fade out after some time
                textComponent.DOFade(0, 0.7f).SetDelay(1f).OnComplete(() => Destroy(floatingText));
            }
        }
        else
        {
            Debug.LogWarning("Floating Damage Prefab is not assigned in the inspector.");
        }
    }

    protected virtual void ShowFloatingCritDamageText(float damage)
    {
        if (floatingDamagePrefab != null)
        {
            Vector3 randomOffset = new Vector3(Random.Range(xOffsetRange.x, xOffsetRange.y), Random.Range(yOffsetRange.x, yOffsetRange.y), 0);

            GameObject floatingText = Instantiate(floatingDamagePrefab, transform.position + Vector3.up + randomOffset, Quaternion.identity);
            TMP_Text textComponent = floatingText.GetComponentInChildren<TMP_Text>();

            if (textComponent != null)
            {
                textComponent.text = damage.ToString("F0");
                textComponent.color = Color.yellow;

                // Scale-up effect
                textComponent.transform.localScale = Vector3.zero; // Start from 0
                textComponent.transform.DOScale(1.5f, 0.5f).SetEase(Ease.OutBack); // Smooth bounce scale-up

                // Optional: Fade out after some time
                textComponent.DOFade(0, 0.7f).SetDelay(1f).OnComplete(() => Destroy(floatingText));
            }
        }
        else
        {
            Debug.LogWarning("Floating Crit Damage Prefab is not assigned in the inspector.");
        }
    }

}
