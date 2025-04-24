using TMPro;
using UnityEngine;

public class PlayerStatsUIManager : MonoBehaviour
{
    public static PlayerStatsUIManager Instance;

    public TextMeshProUGUI HPText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI movementSpeedText;
    public TextMeshProUGUI ArmorText;
    public TextMeshProUGUI dodgeText;
    public TextMeshProUGUI critDamageText;
    public TextMeshProUGUI critChanceText;
    public TextMeshProUGUI HPRegenText;
    public TextMeshProUGUI lifeStealChanceText;
    public TextMeshProUGUI XPText;

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

    private void Start()
    {
        UpdateStats();
    }

    public void UpdateStats()
    {
        HPText.text = PlayerUpgradeManager.Instance.playerMaxHealth.ToString();
        damageText.text = "x" + PlayerUpgradeManager.Instance.playerDamageMultiplier.ToString("0.##");
        movementSpeedText.text = (PlayerUpgradeManager.Instance.playerMovementSpeedMultiplier * PlayerUpgradeManager.Instance.playerSailingSpeed).ToString("0.#");
        ArmorText.text = PlayerUpgradeManager.Instance.playerArmor.ToString();
        dodgeText.text = PlayerUpgradeManager.Instance.playerDodgeChancePercent.ToString("0.##") + "%";
        critDamageText.text = "x" + PlayerUpgradeManager.Instance.playerCritDamageMultiplier.ToString("0.##");
        critChanceText.text = PlayerUpgradeManager.Instance.playerCritChancePercent.ToString("0.##") + "%";
        HPRegenText.text = PlayerUpgradeManager.Instance.playerHealthRegenAmount.ToString("0.##");
        lifeStealChanceText.text = PlayerUpgradeManager.Instance.playerLeechChancePercent.ToString("0.##") + "%";
        XPText.text = "x" + PlayerUpgradeManager.Instance.playerXpMultiplier.ToString("0.##");
    }

}
