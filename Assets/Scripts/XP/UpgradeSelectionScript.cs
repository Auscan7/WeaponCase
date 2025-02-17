using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UpgradeSelectionScript : MonoBehaviour
{
    [System.Serializable]
    public class UpgradePool
    {
        public string poolName;
        public List<Upgrade> upgrades; // List of upgrades in this pool
        public float selectionWeight; // Percentage chance of this pool being chosen
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

    public delegate void UpgradeSelectedHandler(int upgradeID);
    public static event UpgradeSelectedHandler OnUpgradeSelectedEvent;

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

        // Create a HashSet to track already selected upgrades
        HashSet<int> selectedUpgradeIDs = new HashSet<int>();

        for (int i = 0; i < upgradeCards.Length; i++)
        {
            Upgrade selectedUpgrade = null;

            // Try selecting a unique upgrade
            int attempts = 0; // Prevent infinite loops if pools are too small
            while (attempts < 100) // Arbitrary limit to avoid potential infinite loops
            {
                selectedUpgrade = SelectRandomUpgrade();
                if (selectedUpgrade != null && !selectedUpgradeIDs.Contains(selectedUpgrade.UpgradeID))
                {
                    // Add the unique UpgradeID to the HashSet
                    selectedUpgradeIDs.Add(selectedUpgrade.UpgradeID);
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
        // Calculate the total weight
        float totalWeight = 0f;
        foreach (var pool in upgradePools)
        {
            totalWeight += pool.selectionWeight;
        }

        // Generate a random value between 0 and totalWeight
        float randomValue = Random.Range(0, totalWeight);
        float cumulativeWeight = 0f;

        // Determine which pool to select based on the random value
        foreach (var pool in upgradePools)
        {
            cumulativeWeight += pool.selectionWeight;
            if (randomValue <= cumulativeWeight)
            {
                // Select a random upgrade from this pool
                if (pool.upgrades.Count > 0)
                {
                    int randomIndex = Random.Range(0, pool.upgrades.Count);
                    return pool.upgrades[randomIndex];
                }
                else
                {
                    Debug.LogWarning($"Upgrade pool '{pool.poolName}' is empty.");
                }
                break;
            }
        }

        Debug.LogWarning("Failed to select an upgrade. Check your upgrade pool setup.");
        return null;
    }

    private void OnUpgradeSelected(Button clickedCard)
    {
        int selectedIndex = System.Array.IndexOf(upgradeCards, clickedCard);
        if (selectedIndex != -1)
        {
            Upgrade selectedUpgrade = selectedUpgrades[selectedIndex];

            OnUpgradeSelectedEvent?.Invoke(selectedUpgrade.UpgradeID);
            PlayerLevelSystem.instance.CloseUpgradeScreen();
        }
    }
}
