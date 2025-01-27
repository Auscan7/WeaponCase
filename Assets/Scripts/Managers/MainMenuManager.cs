using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public Button playButton;
    public Button arsenalButton;

    public GameObject comingSoonText;

    public Vector3 spawnPos;

    public void PLay()
    {
        AudioManager.instance.PlaySoundSFX(AudioManager.instance.UIClickSFX);
        SceneManager.LoadScene("Level1");
    }

    public void Arsenal()
    {
        if (comingSoonText != null)
        {
            GameObject floatingText = Instantiate(comingSoonText, spawnPos, Quaternion.identity);
            TMP_Text textComponent = floatingText.GetComponentInChildren<TMP_Text>();

            if (textComponent != null)
            {
                textComponent.text = "Coming Soon...";
                textComponent.color = Color.white;
            }
        }
    }
}
