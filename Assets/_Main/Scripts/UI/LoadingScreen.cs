using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    public GameObject loadingScreen;
    public TextMeshProUGUI loadingText;
    public float minimumLoadTime = 3f; // Minimum time to show the loading screen (in seconds)

    void Start()
    {
        // Optionally, hide the loading screen at the start
        loadingScreen.SetActive(false);
    }

    public void LoadSceneWithLoadingScreen(string sceneName, bool waitForAnimation)
    {
        StartCoroutine(LoadAsync(sceneName, waitForAnimation));
    }

    private IEnumerator LoadAsync(string sceneName, bool w)
    {
        loadingScreen.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        if (!w)
        {
            operation.allowSceneActivation = true;
            StartCoroutine(UpdateLoadingText());
        }
        else if (w)
        {
            operation.allowSceneActivation = false;
            StartCoroutine(UpdateLoadingText());

            float elapsedTime = 0f;

            // While loading, check for progress and update the text if needed
            while (!operation.isDone && w)
            {
                // If the loading progress reaches 90% (i.e., almost done)
                if (operation.progress >= 0.9f)
                {
                    // Start activating the scene after the minimum load time has passed
                    if (elapsedTime >= minimumLoadTime)
                    {
                        operation.allowSceneActivation = true;
                    }
                }
                elapsedTime += Time.deltaTime;

                yield return null;
            }
            loadingScreen.SetActive(false);
        }
    }

    public IEnumerator UpdateLoadingText()
    {
        while (true)
        {
            loadingText.text = "Loading.";
            yield return new WaitForSeconds(0.5f);
            loadingText.text = "Loading..";
            yield return new WaitForSeconds(0.5f);
            loadingText.text = "Loading...";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
