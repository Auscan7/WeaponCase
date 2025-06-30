using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using static Upgrade;

public class UpgradeSelectionScript : MonoBehaviour
{
    [System.Serializable]
    public class UpgradePool
    {
        public string poolName;
        public List<Upgrade> upgrades;
        public float selectionWeight;
    }

    public Button[] upgradeCards;
    public TMP_Text[] upgradeDescriptions;
    public TMP_Text[] negativeUpgradeDescriptions;
    public TMP_Text[] upgradeNames;
    public Image[] upgradeIcons;
    public Image[] weaponCardIcon;
    public Image[] upgradeBorders;

    [Header("Upgrade Pools")]
    public List<UpgradePool> upgradePools;

    private Upgrade[] selectedUpgrades;

    private void Start()
    {
        foreach (Button card in upgradeCards)
        {
            card.onClick.AddListener(() => OnUpgradeSelected(card));
        }
    }

    public void SetUpgradeCards()
    {
        selectedUpgrades = new Upgrade[upgradeCards.Length];
        HashSet<Upgrade> selectedUpgradeSet = new HashSet<Upgrade>();

        bool forceWeapon = false;
        List<Upgrade> weaponEligibleUpgrades = GetEligibleUpgradesByType(Upgrade.UpgradeType.Weapon);

        // Weapon quota logic (e.g., 3 weapons in first 10 level-ups)
        if (PlayerLevelSystem.instance.CurrentLevel <= PlayerLevelSystem.instance.weaponQuotaWindow)
        {
            PlayerLevelSystem.instance.levelsCheckedForWeapon++;

            int levelsRemaining = PlayerLevelSystem.instance.weaponQuotaWindow - PlayerLevelSystem.instance.levelsCheckedForWeapon;
            int offersRemaining = PlayerLevelSystem.instance.minWeaponOffersInQuota - PlayerLevelSystem.instance.currentWeaponOffers;

            if (offersRemaining > 0 && offersRemaining >= levelsRemaining)
            {
                forceWeapon = true;
            }
            else
            {
                forceWeapon = Random.value < 0.35f; // 35% chance if not forced
            }
        }

        // Add a weapon upgrade if needed
        if (forceWeapon && weaponEligibleUpgrades.Count > 0)
        {
            Upgrade forcedWeapon = weaponEligibleUpgrades[Random.Range(0, weaponEligibleUpgrades.Count)];
            selectedUpgrades[0] = forcedWeapon;
            selectedUpgradeSet.Add(forcedWeapon);
            AssignUpgradeCardUI(0, forcedWeapon);
            PlayerLevelSystem.instance.currentWeaponOffers++;
        }

        for (int i = selectedUpgrades[0] != null ? 1 : 0; i < upgradeCards.Length; i++)
        {
            Upgrade selectedUpgrade = null;
            int attempts = 0;

            while (attempts < 100)
            {
                selectedUpgrade = SelectRandomUpgrade();
                if (selectedUpgrade != null && !selectedUpgradeSet.Contains(selectedUpgrade))
                {
                    selectedUpgradeSet.Add(selectedUpgrade);
                    break;
                }
                attempts++;
            }

            if (selectedUpgrade != null)
            {
                selectedUpgrades[i] = selectedUpgrade;
                AssignUpgradeCardUI(i, selectedUpgrade);
            }
            else
            {
                Debug.LogWarning("Failed to select a unique upgrade.");
            }
        }
    }

    private void AssignUpgradeCardUI(int index, Upgrade upgrade)
    {
        upgradeNames[index].text = upgrade.upgradeName;
        upgradeDescriptions[index].text = upgrade.positiveDescription;
        negativeUpgradeDescriptions[index].text = upgrade.negativeDescription;

        upgradeIcons[index].sprite = upgrade.icon;

        if (upgrade.upgradeType == Upgrade.UpgradeType.Weapon)
        {
            weaponCardIcon[index].gameObject.SetActive(true);
        }
        else
        {
            weaponCardIcon[index].gameObject.SetActive(false);
        }

        upgradeBorders[index].sprite = upgrade.border;
    }

    private List<Upgrade> GetEligibleUpgradesByType(Upgrade.UpgradeType type)
    {
        List<Upgrade> eligible = new List<Upgrade>();

        foreach (var pool in upgradePools)
        {
            foreach (var upgrade in pool.upgrades)
            {
                if (upgrade.upgradeType != type) continue;

                if (upgrade is IConditionalUpgrade conditional)
                {
                    if (conditional.CanOffer())
                        eligible.Add(upgrade);
                }
                else
                {
                    eligible.Add(upgrade);
                }
            }
        }

        return eligible;
    }

    private Upgrade SelectRandomUpgrade()
    {
        float totalWeight = 0f;
        List<UpgradePool> validPools = new List<UpgradePool>();

        foreach (var pool in upgradePools)
        {
            bool hasEligible = false;

            foreach (var upgrade in pool.upgrades)
            {
                if (upgrade is IConditionalUpgrade conditional)
                {
                    if (conditional.CanOffer())
                    {
                        hasEligible = true;
                        break;
                    }
                }
                else
                {
                    hasEligible = true;
                    break;
                }
            }

            if (hasEligible)
            {
                totalWeight += pool.selectionWeight;
                validPools.Add(pool);
            }
        }

        if (totalWeight == 0f)
        {
            Debug.LogWarning("No valid upgrade pools found.");
            return null;
        }

        float randomValue = Random.Range(0, totalWeight);
        float cumulativeWeight = 0f;

        UpgradePool selectedPool = null;

        foreach (var pool in validPools)
        {
            cumulativeWeight += pool.selectionWeight;
            if (randomValue <= cumulativeWeight)
            {
                selectedPool = pool;
                break;
            }
        }

        if (selectedPool == null)
        {
            Debug.LogWarning("Failed to select an upgrade pool.");
            return null;
        }

        List<Upgrade> eligibleUpgrades = new List<Upgrade>();
        foreach (var upgrade in selectedPool.upgrades)
        {
            if (upgrade is IConditionalUpgrade conditional)
            {
                if (conditional.CanOffer())
                    eligibleUpgrades.Add(upgrade);
            }
            else
            {
                eligibleUpgrades.Add(upgrade);
            }
        }

        if (eligibleUpgrades.Count == 0)
        {
            Debug.LogWarning($"No eligible upgrades in pool {selectedPool.poolName}");
            return null;
        }

        return eligibleUpgrades[Random.Range(0, eligibleUpgrades.Count)];
    }

    private void OnUpgradeSelected(Button clickedCard)
    {
        int selectedIndex = System.Array.IndexOf(upgradeCards, clickedCard);
        if (selectedIndex != -1)
        {
            Upgrade selectedUpgrade = selectedUpgrades[selectedIndex];
            selectedUpgrade.ApplyUpgrade(PlayerUpgradeManager.Instance);
            PlayerStatsUIManager.Instance.UpdateStats();
            PlayerLevelSystem.instance.CloseUpgradeScreen();
        }
    }
}
