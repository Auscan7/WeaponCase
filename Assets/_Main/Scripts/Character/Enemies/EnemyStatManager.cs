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

        //crit
        Crit(damage);

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0); // Ensure health doesn't go below zero

        //leech
        Leech(damage, PlayerUpgradeManager.Instance.playerLeechAmountPercent);

        healthBar.fillAmount = currentHealth / maxHealth;
        FloatingTextManager.Instance.ShowFloatingText(transform.position, damage.ToString("F0"), Color.white, 1.1f, 0.35f, 0.6f);

        // required for hiding the hp bar
        damageTimer = 0f;
    }

    // Check if enemy takes a critical hit
    private void Crit(float damage)
    {
        int critChance = Random.Range(0, 101);

        if (critChance < PlayerUpgradeManager.Instance.playerCritChancePercent)
        {
            float critDamage = (damage += damage / 10 * PlayerUpgradeManager.Instance.playerCritDamageMultiplier);
            currentHealth -= critDamage;
            currentHealth = Mathf.Max(currentHealth, 0); // Ensure health doesn't go below zero

            //leech
            Leech(critDamage, PlayerUpgradeManager.Instance.playerLeechAmountPercent);
            FloatingTextManager.Instance.ShowFloatingText(transform.position, critDamage.ToString("F0"), Color.yellow, 1.75f, 0.2f, 0.7f);

            return; // No crit damage applied
        }
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

            
            FloatingTextManager.Instance.ShowFloatingText(player.position, leechAmount.ToString("F0"), Color.green, 0.9f, 0.5f, 0.6f);
        }
    }
}
