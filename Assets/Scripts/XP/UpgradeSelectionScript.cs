using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic; // Required for List

public class UpgradeSelectionScript : MonoBehaviour
{
    public Button[] upgradeCards; // Buttons for upgrade cards
    public TMP_Text[] upgradeDescriptions; // Descriptions for upgrade cards
    public TMP_Text[] negativeUpgradeDescriptions; // Descriptions for upgrade cards
    public TMP_Text[] upgradeNames; // Names for upgrade cards
    public Image[] upgradeIcons; // Icons for upgrade cards
    public Image[] upgradeBorders; // Icons for upgrade cards

    [Header("Upgrade Pool")]
    public Upgrade[] upgradePool; // Array of ScriptableObjects representing upgrades

    private Upgrade[] selectedUpgrades; // Stores the upgrades selected for this level

    // Define the delegate
    public delegate void UpgradeSelectedHandler(int upgradeID);

    // Declare the event
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
        Debug.Log("Setting Upgrade Cards...");  // Add this line to verify
                                                // Create a temporary list to ensure unique selections
        List<Upgrade> availableUpgrades = new List<Upgrade>(upgradePool);
        selectedUpgrades = new Upgrade[upgradeCards.Length];

        // Use a HashSet to track selected UpgradeIDs
        HashSet<int> selectedUpgradeIDs = new HashSet<int>();

        for (int i = 0; i < upgradeCards.Length; i++)
        {
            if (availableUpgrades.Count == 0)
            {
                Debug.LogWarning("Not enough unique upgrades in the pool to fill all cards.");
                break;
            }

            // Select a random upgrade and check for duplicate UpgradeID
            Upgrade selectedUpgrade = null;
            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, availableUpgrades.Count);
                selectedUpgrade = availableUpgrades[randomIndex];
            }
            while (selectedUpgradeIDs.Contains(selectedUpgrade.UpgradeID)); // Check for duplicates

            // Add the selected UpgradeID to the HashSet
            selectedUpgradeIDs.Add(selectedUpgrade.UpgradeID);

            // Remove the selected upgrade from the available pool
            availableUpgrades.RemoveAt(randomIndex);

            // Assign the upgrade to the selected slot
            selectedUpgrades[i] = selectedUpgrade;

            // Assign name, description, and icon to the card
            upgradeNames[i].text = selectedUpgrade.upgradeName;
            upgradeDescriptions[i].text = selectedUpgrade.positiveDescription;
            negativeUpgradeDescriptions[i].text = selectedUpgrade.negativeDescription;
            upgradeIcons[i].sprite = selectedUpgrade.icon;
            upgradeBorders[i].sprite = selectedUpgrade.border;
        }
    }


    private void OnUpgradeSelected(Button clickedCard)
    {
        int selectedIndex = System.Array.IndexOf(upgradeCards, clickedCard);
        if (selectedIndex != -1)
        {
            Upgrade selectedUpgrade = selectedUpgrades[selectedIndex];
            Debug.Log($"Selected Upgrade: {selectedUpgrade.upgradeName}");

            // Trigger the event and pass the upgrade ID
            OnUpgradeSelectedEvent?.Invoke(selectedUpgrade.UpgradeID);

            // Close the upgrade screen
            PlayerLevelSystem.instance.CloseUpgradeScreen();
        }
    }
}
