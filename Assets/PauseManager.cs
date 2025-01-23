using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance;

    public Button continueButton;
    public Button menuButton;
    public GameObject pauseMenu;
    [HideInInspector] public bool isPauseMenuOpen = false;

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
        Time.timeScale = 0;
    }

    public void UnPauseGame()
    {
        Time.timeScale = 1;
    }

    public void OpenPauseMenu()
    {
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
}
