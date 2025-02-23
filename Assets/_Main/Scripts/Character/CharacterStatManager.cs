using UnityEngine;
using UnityEngine.UI;


public class CharacterStatManager : MonoBehaviour
{
    [HideInInspector]public BasicEnemyMovement basicEnemyMovement;

    private bool isDead = false;

    [Header("Health Bar")]
    [HideInInspector] public float currentHealth;
    [HideInInspector] public int maxHealth;
    [SerializeField] public Image healthBar;
    [SerializeField] public GameObject healthBarParent;

    protected virtual void Awake()
    {
        basicEnemyMovement = GetComponent<BasicEnemyMovement>();
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
            isDead = true;
        }
    }

    public virtual void TakeDamage(float damage)
    {

    }

    public void ResetEnemy()
    {
        currentHealth = maxHealth;
        isDead = false;
        healthBar.fillAmount = 1f; // Reset health bar
        healthBarParent.SetActive(false); // Hide health bar
    }

    public virtual void HandleDeath()
    {
        
    }
}
