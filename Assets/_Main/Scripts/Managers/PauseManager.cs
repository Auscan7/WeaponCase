using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance;
    public LoadingScreen loadingScreen; // Reference to the LoadingScreen script
    public string sceneToLoad = "Main Menu"; // Set this to whatever scene you want to load

    public Button continueButton;
    public Button menuButton;
    public GameObject pauseMenu;
    [HideInInspector] public bool isPauseMenuOpen = false;
    [HideInInspector] public bool isPaused = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PauseGame()
    {
        if(isPaused)
            return;

        Time.timeScale = 0;
        isPaused = true;
    }

    public void UnPauseGame()
    {
        if(!isPaused)
            return;

        Time.timeScale = 1;
        isPaused = false;
    }

    public void OpenPauseMenu()
    {
        if (isPaused)
            return;

        AudioManager.instance.PlaySoundSFX(AudioManager.instance.UIClickSFX);
        PauseGame();
        pauseMenu.SetActive(true);
        isPauseMenuOpen = true;
    }

    public void Continue()
    {
        UnPauseGame();
        AudioManager.instance.PlaySoundSFX(AudioManager.instance.UIClickSFX);
        pauseMenu.SetActive(false);
        isPauseMenuOpen = false;        
    }

    public void LoadMainMenu()
    {
        UnPauseGame();
        AudioManager.instance.PlaySoundSFX(AudioManager.instance.UIClickSFX);
        loadingScreen.loadingScreen.SetActive(true);
        SceneManager.LoadScene(sceneToLoad);
    }
}
