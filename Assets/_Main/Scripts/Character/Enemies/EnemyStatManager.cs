using UnityEngine;
using System.Collections;

public class EnemyStatManager : CharacterStatManager
{
    private EnemyManager enemyManager;
    GemDrop gemDrop;
    EnemyItemDrop itemDrop;
    private Material originalMaterial;
    private Material flashMaterial;
    private SpriteRenderer spriteRenderer;

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
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        
        if (spriteRenderer != null)
        {
            originalMaterial = spriteRenderer.material;
            // Create a new instance of the flash material
            flashMaterial = new Material(Shader.Find("Custom/FlashShader"));
            flashMaterial.SetColor("_FlashColor", Color.white);
            flashMaterial.SetFloat("_FlashAmount", 0f);
        }
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

    public override void TakeDamage(float damage, Color color = default)
    {
        base.TakeDamage(damage, default);

        AudioManager.instance.PlaySoundSFX(AudioManager.instance.enemyTakeDamageSFX);

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

        // Flash effect
        if (spriteRenderer != null && flashMaterial != null)
        {
            StartCoroutine(FlashDamageEffect(color));
        }
    }

    private IEnumerator FlashDamageEffect(Color color)
    {
        if (spriteRenderer != null && flashMaterial != null)
        {
            // Switch to flash material
            spriteRenderer.material = flashMaterial;

            flashMaterial.SetColor("_FlashColor", color);

            // Flash in
            float flashInDuration = 0.1f;
            float flashOutDuration = 0.1f;
            
            // Flash in
            for (float t = 0; t < flashInDuration; t += Time.deltaTime)
            {
                float normalizedTime = t / flashInDuration;
                flashMaterial.SetFloat("_FlashAmount", normalizedTime);
                yield return null;
            }
            
            // Flash out
            for (float t = 0; t < flashOutDuration; t += Time.deltaTime)
            {
                float normalizedTime = t / flashOutDuration;
                flashMaterial.SetFloat("_FlashAmount", 1f - normalizedTime);
                yield return null;
            }
            
            // Reset flash amount
            flashMaterial.SetFloat("_FlashAmount", 0f);
            
            // Return to original material
            spriteRenderer.material = originalMaterial;
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

            
            FloatingTextManager.Instance.ShowFloatingText(player.position, leechAmount.ToString("0.#"), Color.green, 0.9f, 0.5f, 0.6f);
        }
    }
}
