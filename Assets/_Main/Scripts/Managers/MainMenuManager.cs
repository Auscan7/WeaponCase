using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public Button playButton;
    public Button exitButton;
    public GameObject exitPopUp; // Reference to the exit popup UI element
    public LoadingScreen loadingScreen; // Reference to the LoadingScreen script
    public string sceneToLoad = "Level1"; // Set this to whatever scene you want to load

    private bool isExitUIOpen = false;

    public GameObject MainMenuButtons;

    public Vector3 spawnPos;

    private void Awake()
    {
        Camera.main.transform.position = new Vector3(0, 0, -10);
    }

    private void Start()
    {
        // Adding onClick listener to playButton
        playButton.onClick.AddListener(OnPlayButtonClicked);
        // Adding onClick listener to exitButton
        exitButton.onClick.AddListener(OnExitButtonClicked);

        // Hide the exit popup initially
        exitPopUp.SetActive(false);
    }

    private void Update()
    {
        if (isExitUIOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            isExitUIOpen = false;
            exitPopUp.SetActive(false);
        }
    }

    public void OnExitButtonClicked()
    {
        // Show the exit popup
        exitPopUp.SetActive(true);
        isExitUIOpen = true; // Set the flag to indicate the exit UI is open
        AudioManager.instance.PlaySoundSFX(AudioManager.instance.UIClickSFX); // Play click sound
    }

    public void OnExitConfirmButtonClicked()
    {
        // Exit the application
        AudioManager.instance.PlaySoundSFX(AudioManager.instance.UIClickSFX); // Play click sound
        Application.Quit();
    }

    public void OnExitCancelButtonClicked()
    {
        // Hide the exit popup
        exitPopUp.SetActive(false);
        isExitUIOpen = false; // Reset the flag
        AudioManager.instance.PlaySoundSFX(AudioManager.instance.UIClickSFX); // Play click sound
    }

    public void OnPlayButtonClicked()
    {
        // Call the method in LoadingScreen to show the loading screen
        // Pass the sceneToLoad to the loading screen
        AudioManager.instance.PlaySoundSFX(AudioManager.instance.UIClickSFX); // Play click sound
        loadingScreen.LoadSceneWithLoadingScreen(sceneToLoad, waitForAnimation: true);
    }
}
