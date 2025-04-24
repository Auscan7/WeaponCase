using TMPro;
using UnityEngine;
using System.Collections;

public class PlayerStatsUIManager : MonoBehaviour
{
    public static PlayerStatsUIManager Instance;

    public Color baseStatColor = Color.white;

    [System.Serializable]
    public class StatText
    {
        public TextMeshProUGUI text;
        [HideInInspector] public string lastValueString = "";
        [HideInInspector] public float lastValueFloat = float.NaN;
    }

    public StatText HPText;
    public StatText damageText;
    public StatText movementSpeedText;
    public StatText ArmorText;
    public StatText dodgeText;
    public StatText critDamageText;
    public StatText critChanceText;
    public StatText HPRegenText;
    public StatText lifeStealChanceText;
    public StatText XPText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator Start()
    {
        UpdateStats();
        yield return new WaitForEndOfFrame(); // Wait one frame
        ResetAllStatColors();
    }

    public void UpdateStats()
    {
        float hp = PlayerUpgradeManager.Instance.playerMaxHealth;
        UpdateStatText(HPText, hp, hp.ToString("F0"));

        float dmg = PlayerUpgradeManager.Instance.playerDamageMultiplier;
        UpdateStatText(damageText, dmg, "x" + dmg.ToString("F2"));

        float moveSpeed = PlayerUpgradeManager.Instance.playerMovementSpeedMultiplier * PlayerUpgradeManager.Instance.playerSailingSpeed;
        UpdateStatText(movementSpeedText, moveSpeed, moveSpeed.ToString("F1"));

        float armor = PlayerUpgradeManager.Instance.playerArmor;
        float reductionPercent = (armor * 7.5f) / (100 + (armor * 7.5f)) * 100f;
        UpdateStatText(ArmorText, armor, $"{armor:F0} ({reductionPercent:F1}%)");

        float dodge = PlayerUpgradeManager.Instance.playerDodgeChancePercent;
        UpdateStatText(dodgeText, dodge, $"{dodge:F2}%");

        float critDmg = PlayerUpgradeManager.Instance.playerCritDamageMultiplier;
        UpdateStatText(critDamageText, critDmg, "x" + critDmg.ToString("F2"));

        float critChance = PlayerUpgradeManager.Instance.playerCritChancePercent;
        UpdateStatText(critChanceText, critChance, $"{critChance:F2}%");

        float regen = PlayerUpgradeManager.Instance.playerHealthRegenAmount;
        UpdateStatText(HPRegenText, regen, $"+ {regen:F2}/s");

        float leech = PlayerUpgradeManager.Instance.playerLeechChancePercent;
        UpdateStatText(lifeStealChanceText, leech, $"{leech:F2}%");

        float xpMult = PlayerUpgradeManager.Instance.playerXpMultiplier;
        UpdateStatText(XPText, xpMult, "x" + xpMult.ToString("F2"));
    }

    private void UpdateStatText(StatText stat, float actualValue, string displayValue)
    {
        if (!Mathf.Approximately(actualValue, stat.lastValueFloat))
        {
            Color colorToUse = Color.green;

            if (!float.IsNaN(stat.lastValueFloat) && actualValue < stat.lastValueFloat)
            {
                colorToUse = Color.red;
            }

            stat.text.text = displayValue;
            StartCoroutine(SmoothColorChange(stat.text, stat.text.color, colorToUse));

            stat.lastValueFloat = actualValue;
            stat.lastValueString = displayValue;
        }
    }

    private IEnumerator SmoothColorChange(TextMeshProUGUI text, Color fromColor, Color toColor)
    {
        float duration = 0.6f;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            text.color = Color.Lerp(fromColor, toColor, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        text.color = toColor;

        yield return new WaitForSeconds(0.5f);

        Color startColor = text.color;
        timeElapsed = 0f;
        while (timeElapsed < duration)
        {
            text.color = Color.Lerp(startColor, baseStatColor, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        text.color = baseStatColor;
    }

    private void ResetAllStatColors()
    {
        ResetColor(HPText);
        ResetColor(damageText);
        ResetColor(movementSpeedText);
        ResetColor(ArmorText);
        ResetColor(dodgeText);
        ResetColor(critDamageText);
        ResetColor(critChanceText);
        ResetColor(HPRegenText);
        ResetColor(lifeStealChanceText);
        ResetColor(XPText);
    }

    private void ResetColor(StatText stat)
    {
        stat.text.color = baseStatColor;
    }
}
