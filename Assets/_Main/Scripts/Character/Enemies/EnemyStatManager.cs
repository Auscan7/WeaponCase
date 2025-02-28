using UnityEngine;

public class EnemyStatManager : CharacterStatManager
{
    private EnemyManager enemyManager;
    GemDrop gemDrop;
    EnemyItemDrop itemDrop;

    public GameObject enemyPrefabReference; // Set this to the prefab used in the pool

    // hide health bar
    private float damageTimer = 0f;
    private float hideHealthBarDelay = 3f;

    protected override void Awake()
    {
        base.Awake();

        enemyManager = GetComponent<EnemyManager>();
        gemDrop = GetComponent<GemDrop>();
        itemDrop = GetComponent<EnemyItemDrop>();
    }

    public override void HandleDeath()
    {
        gemDrop.DropGem();
        itemDrop.DropItem();

        // Return enemy to the pool
        EnemyPoolManager.Instance.ReturnEnemy(gameObject, enemyPrefabReference);
    }

    protected override void Update()
    {
        base.Update();

        if(healthBarParent == null)
            return;

        if (currentHealth == maxHealth)
        {
            healthBarParent.SetActive(false);
        }
        else
        {
            damageTimer += Time.deltaTime;

            if (damageTimer >= hideHealthBarDelay)
            {
                healthBarParent.SetActive(false);
            }
            else
            {
                healthBarParent.SetActive(true);
            }
        }
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        int critChance = Random.Range(0, 101);
        bool isCrit = critChance < PlayerUpgradeManager.Instance.playerCritChancePercent;

        float finalDamage = damage;

        if (isCrit)
        {
            finalDamage += damage / 10 * PlayerUpgradeManager.Instance.playerCritDamageMultiplier;
        }

        currentHealth -= finalDamage;
        currentHealth = Mathf.Max(currentHealth, 0); // Ensure health doesn't go below zero

        // Leech
        Leech(finalDamage, PlayerUpgradeManager.Instance.playerLeechAmountPercent);

        healthBar.fillAmount = currentHealth / maxHealth;

        // Show only one floating text based on whether it was a crit
        Color textColor = isCrit ? Color.yellow : Color.white;
        float textSize = isCrit ? 1.75f : 1.1f;
        float duration = isCrit ? 0.2f : 0.35f;
        float fadeSpeed = isCrit ? 0.7f : 0.6f;

        FloatingTextManager.Instance.ShowFloatingText(transform.position, finalDamage.ToString("F0"), textColor, textSize, duration, fadeSpeed);

        // Reset damage timer for hiding the health bar
        damageTimer = 0f;
    }

    // check if player leeches health
    private void Leech(float damage, float leechAmount)
    {        
        leechAmount = damage / 10 * leechAmount;
        Transform player = enemyManager.player;

        if (player == null)
            return;

        int leechChance = Random.Range(0, 101);

        if (leechChance < PlayerUpgradeManager.Instance.playerLeechChancePercent && PlayerUpgradeManager.Instance.playerCurrentHealth != PlayerUpgradeManager.Instance.playerMaxHealth)
        {
            PlayerUpgradeManager.Instance.playerCurrentHealth = Mathf.Min(
                   PlayerUpgradeManager.Instance.playerMaxHealth,
                   PlayerUpgradeManager.Instance.playerCurrentHealth + leechAmount);

            
            FloatingTextManager.Instance.ShowFloatingText(player.position, leechAmount.ToString("0.#"), Color.green, 0.9f, 0.5f, 0.6f);
        }
    }
}
