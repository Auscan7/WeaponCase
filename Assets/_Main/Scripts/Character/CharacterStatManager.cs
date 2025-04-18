using UnityEngine;
using UnityEngine.UI;


public class CharacterStatManager : MonoBehaviour
{
    [HideInInspector]public EnemyMovementManager enemyMovementManager;

    private bool isDead = false;

    [Header("Health Bar")]
    public float currentHealth;
    public int maxHealth;
    [SerializeField] public Image healthBar;
    [SerializeField] public GameObject healthBarParent;

    protected virtual void Awake()
    {
        enemyMovementManager = GetComponent<EnemyMovementManager>();
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

    public virtual void TakeDamage(float damage, Color color = default)
    {
        if (color == default)
        {
            color = Color.white;
        }
        // Add your damage handling logic here  
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
