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
        public List<Upgrade> upgrades; // List of upgrades in this pool
        public float selectionWeight;  // Percentage chance of this pool being chosen
    }

    public Button[] upgradeCards;
    public TMP_Text[] upgradeDescriptions;
    public TMP_Text[] negativeUpgradeDescriptions;
    public TMP_Text[] upgradeNames;
    public Image[] upgradeIcons;
    public Image[] upgradeBorders;

    [Header("Upgrade Pools")]
    public List<UpgradePool> upgradePools; // List of upgrade pools

    private Upgrade[] selectedUpgrades;

    private void Start()
    {
        // Attach each button a listener that calls OnUpgradeSelected for that specific button.
        foreach (Button card in upgradeCards)
        {
            card.onClick.AddListener(() => OnUpgradeSelected(card));
        }
    }

    public void SetUpgradeCards()
    {
        selectedUpgrades = new Upgrade[upgradeCards.Length];

        // Use a HashSet to track already selected upgrades (using reference equality)
        HashSet<Upgrade> selectedUpgradeSet = new HashSet<Upgrade>();

        for (int i = 0; i < upgradeCards.Length; i++)
        {
            Upgrade selectedUpgrade = null;
            int attempts = 0; // Prevent infinite loops if pools are too small

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
                // Assign the upgrade to the current card
                selectedUpgrades[i] = selectedUpgrade;
                upgradeNames[i].text = selectedUpgrade.upgradeName;
                upgradeDescriptions[i].text = selectedUpgrade.positiveDescription;
                negativeUpgradeDescriptions[i].text = selectedUpgrade.negativeDescription;
                upgradeIcons[i].sprite = selectedUpgrade.icon;
                upgradeBorders[i].sprite = selectedUpgrade.border;
            }
            else
            {
                Debug.LogWarning("Failed to select a unique upgrade. Check your upgrade pool setup.");
            }
        }
    }

    private Upgrade SelectRandomUpgrade()
    {
        // Calculate the total weight from pools that have at least one eligible upgrade.
        float totalWeight = 0f;
        List<UpgradePool> validPools = new List<UpgradePool>();

        foreach (var pool in upgradePools)
        {
            // Check if the pool has any eligible upgrades.
            bool hasEligible = false;
            foreach (var upgrade in pool.upgrades)
            {
                // If the upgrade implements IConditionalUpgrade, check eligibility.
                if (upgrade is IConditionalUpgrade conditionalUpgrade)
                {
                    if (conditionalUpgrade.CanOffer())
                    {
                        hasEligible = true;
                        break;
                    }
                }
                else
                {
                    // If not conditional, it's always eligible.
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

        // Pick a pool based on selection weight.
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

        // From the selected pool, filter eligible upgrades.
        List<Upgrade> eligibleUpgrades = new List<Upgrade>();
        foreach (var upgrade in selectedPool.upgrades)
        {
            if (upgrade is IConditionalUpgrade conditionalUpgrade)
            {
                if (conditionalUpgrade.CanOffer())
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

        int randomIndex = Random.Range(0, eligibleUpgrades.Count);
        return eligibleUpgrades[randomIndex];
    }


    private void OnUpgradeSelected(Button clickedCard)
    {
        int selectedIndex = System.Array.IndexOf(upgradeCards, clickedCard);
        if (selectedIndex != -1)
        {
            Upgrade selectedUpgrade = selectedUpgrades[selectedIndex];

            // Apply the upgrade effect directly using the new logic.
            selectedUpgrade.ApplyUpgrade(UpgradeManager.Instance);

            PlayerLevelSystem.instance.CloseUpgradeScreen();
        }
    }
}