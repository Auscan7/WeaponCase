using UnityEngine;
using UnityEngine.UI;
using TMPro; // Include TextMesh Pro namespace

public class UpgradeSelectionScript : MonoBehaviour
{
    public Button[] upgradeCards; // Reference to the upgrade card buttons
    public TMP_Text[] upgradeDescriptions; // TextMesh Pro Text for each card's description
    public TMP_Text upgradeInfoText; // Display info about the selected upgrade (optional)

    private void Start()
    {
        // Check if the upgradeCards array is properly initialized
        if (upgradeCards == null || upgradeCards.Length != 3)
        {
            Debug.LogError("Upgrade cards are not properly assigned!");
            return;
        }

        foreach (Button card in upgradeCards)
        {
            card.onClick.AddListener(() => OnUpgradeSelected(card)); // Add listener for each upgrade card
        }

        SetUpgradeCards();
    }

    // Called when an upgrade card is clicked
    private void OnUpgradeSelected(Button clickedCard)
    {
        // Handle what happens when an upgrade is selected
        Debug.Log(clickedCard.name + " selected!");

        // Call method from PlayerLevelSystem to apply the upgrade
        // Example: PlayerLevelSystem.instance.ApplyUpgrade(clickedCard.name);

        // Close the upgrade screen
        PlayerLevelSystem.instance.CloseUpgradeScreen();
    }

    // Set up the upgrade cards for the player to choose from
    private void SetUpgradeCards()
    {
        // Example setup, you can customize it based on your level design and progression
        if (upgradeDescriptions != null && upgradeDescriptions.Length == 3)
        {
            upgradeDescriptions[0].text = "Health Boost +10";
            upgradeDescriptions[1].text = "Damage Boost +5";
            upgradeDescriptions[2].text = "Speed Boost +1";
        }
        else
        {
            Debug.LogWarning("Upgrade descriptions are not properly assigned.");
        }
    }
}
