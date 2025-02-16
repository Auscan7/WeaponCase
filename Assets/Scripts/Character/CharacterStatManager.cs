using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;


public class CharacterStatManager : MonoBehaviour
{


    [Header("Health Bar")]
    [SerializeField] public float currentHealth;
    [SerializeField] public int maxHealth;
    [SerializeField] public Image healthBar;
    [SerializeField] public GameObject healthBarParent;

    protected virtual void Awake()
    {

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
            FloatingTextManager.Instance.ShowFloatingText(transform.position, critDamage.ToString("F0"), Color.yellow, 1.75f, 0.2f, 0.7f);
            return; // No crit damage applied
        }

        currentHealth -= damage;

        healthBar.fillAmount = currentHealth / maxHealth;

        FloatingTextManager.Instance.ShowFloatingText(transform.position, damage.ToString("F0"), Color.white, 1.1f, 0.35f, 0.6f);
    }

    public virtual void HandleDeath()
    {
        Destroy(gameObject);
    }  
}
