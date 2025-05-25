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

    // Poison status tracking
    private bool isPoisoned = false;
    private float poisonTimer = 0f;
    private float poisonDuration = 5f; // Duration of poison effect in seconds
    private Coroutine poisonCoroutine;
    private float poisonDamagePercentage;
    private float poisonMinDamage;
    private float poisonTickInterval;
    private Color poisonColor;

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
            // Store the original material
            originalMaterial = spriteRenderer.material;
            
            // Create a new instance of the flash material
            var shader = Shader.Find("Custom/FlashShader");
            if (shader != null) {
                flashMaterial = new Material(shader);
                flashMaterial.mainTexture = originalMaterial.mainTexture;
                flashMaterial.SetColor("_FlashColor", Color.white);
                flashMaterial.SetFloat("_FlashAmount", 0f);
            } else {
                Debug.LogError("Custom/FlashShader not found! Make sure it is included in the build.");
            }
        }
    }

    protected override void Start()
    {
        float multiplier = DifficultyManager.instance.GetCurrentEnemyHealthMultiplier();
        maxHealth = Mathf.RoundToInt(maxHealth * multiplier);
        currentHealth = maxHealth;
    }


    public override void HandleDeath()
    {
        gemDrop.DropGem();
        itemDrop.DropItem();

        EffectsManager effectsManager = EffectsManager.instance;
        AudioManager audioManager = AudioManager.instance;

        effectsManager.PlayVFX(effectsManager.enemyDeathVFX, transform.position, Quaternion.identity);
        audioManager.PlaySoundSFX(audioManager.enemyDeathSFX);

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

        // Update poison timer
        if (isPoisoned)
        {
            poisonTimer += Time.deltaTime;
            if (poisonTimer >= poisonDuration)
            {
                isPoisoned = false;
                poisonTimer = 0f;
            }
        }
    }

    public void StartPoison(float damagePercent, float minDamage, float duration, float interval, Color damageColor)
    {
        poisonDamagePercentage = damagePercent;
        poisonMinDamage = minDamage;
        poisonTickInterval = interval;
        poisonColor = damageColor;

        // Restart poison timer and coroutine
        if (poisonCoroutine != null)
            StopCoroutine(poisonCoroutine);

        poisonCoroutine = StartCoroutine(PoisonTick(duration));
    }

    private IEnumerator PoisonTick(float duration)
    {
        isPoisoned = true;
        float timer = 0f;

        while (timer < duration && gameObject.activeInHierarchy)
        {
            yield return new WaitForSeconds(poisonTickInterval);
            timer += poisonTickInterval;

            float calculatedDamage = Mathf.Max(maxHealth * (poisonDamagePercentage / 100f), poisonMinDamage);
            TakeDamage(calculatedDamage, poisonColor);
        }

        isPoisoned = false;
        poisonCoroutine = null;
    }


    public void ResetEnemy()
    {
        base.ResetEnemy();

        // Reset poison status
        isPoisoned = false;
        poisonTimer = 0f;

        if (poisonCoroutine != null)
        {
            StopCoroutine(poisonCoroutine);
            poisonCoroutine = null;
        }
    }


    public override void TakeDamage(float damage, Color color = default)
    {
        // If no color is specified, use white
        if (color == default)
        {
            color = Color.white;
        }
        
        AudioManager.instance.PlaySoundSFX(AudioManager.instance.enemyTakeDamageSFX);

        int critChance = Random.Range(0, 101);
        bool isCrit = critChance < PlayerUpgradeManager.Instance.playerCritChancePercent;

        float finalDamage = damage;

        if (isCrit)
        {
            finalDamage *= PlayerUpgradeManager.Instance.playerCritDamageMultiplier;
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
            // Always use the stored original material
            Material currentOriginalMaterial = originalMaterial;
            
            // Switch to flash material
            spriteRenderer.material = flashMaterial;

            // Set flash color with full opacity
            Color flashColor = color;
            flashColor.a = 1f; // Ensure full opacity
            flashMaterial.SetColor("_FlashColor", flashColor);

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
            
            // Always return to the stored original material
            spriteRenderer.material = currentOriginalMaterial;
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
