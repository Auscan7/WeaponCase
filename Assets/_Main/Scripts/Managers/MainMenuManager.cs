using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public Button playButton;
    public LoadingScreen loadingScreen; // Reference to the LoadingScreen script
    public string sceneToLoad = "Level1"; // Set this to whatever scene you want to load

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
    }

    public void OnPlayButtonClicked()
    {
        // Call the method in LoadingScreen to show the loading screen
        // Pass the sceneToLoad to the loading screen
        AudioManager.instance.PlaySoundSFX(AudioManager.instance.UIClickSFX); // Play click sound
        loadingScreen.LoadSceneWithLoadingScreen(sceneToLoad, waitForAnimation: true);
    }
}
