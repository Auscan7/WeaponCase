using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance;

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

        PauseGame();
        pauseMenu.SetActive(true);
        isPauseMenuOpen = true;
    }

    public void Continue()
    {
        UnPauseGame();
        pauseMenu.SetActive(false);
        isPauseMenuOpen = false;        
    }

    public void LoadMainMenu()
    {
        Continue();
        SceneManager.LoadScene("Main Menu");
    }
}
