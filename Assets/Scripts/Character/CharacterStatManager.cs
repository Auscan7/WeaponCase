using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
            }
        }
        else
        {
            Debug.LogWarning("Floating Damage Prefab is not assigned in the inspector.");
        }
    }

}
