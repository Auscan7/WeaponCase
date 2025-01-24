using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public Button playButton;

    public void PLay()
    {
        SceneManager.LoadScene("Level1");
    }
}
