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
    public GameObject MainMenuButtons;

    public Vector3 spawnPos;

    private void Awake()
    {
        Camera.main.transform.position = new Vector3(0, 0, -10);
    }

    public void Play()
    {
        AudioManager.instance.PlaySoundSFX(AudioManager.instance.UIClickSFX);       
        SceneManager.LoadScene("Level1");
        //SceneManager.LoadScene("Test");
    }
}
