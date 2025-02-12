using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public Button playButton;

    public GameObject comingSoonText;
    public GameObject InventoryPanel;
    public GameObject BGImage;
    public GameObject MainMenuButtons;
    public GameObject ReadyButton;

    public Vector3 spawnPos;

    public void PLay()
    {
        AudioManager.instance.PlaySoundSFX(AudioManager.instance.UIClickSFX);
        InventoryPanel.SetActive(true);
        BGImage.SetActive(false);
        MainMenuButtons.SetActive(false);
        ReadyButton.SetActive(true);
    }

    public void Ready()
    {
        AudioManager.instance.PlaySoundSFX(AudioManager.instance.UIClickSFX);
        SceneManager.LoadScene("Level1");
    }
}
