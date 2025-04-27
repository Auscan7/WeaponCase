using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    public GameObject loadingScreen; // Reference to the loading screen canvas
    public TextMeshProUGUI loadingText; // Text for loading message
    public float minimumLoadTime = 3f; // Minimum time to show the loading screen (in seconds)

    void Start()
    {
        // Optionally, hide the loading screen at the start
        loadingScreen.SetActive(false);
    }

    // Call this function when you want to load any scene with a loading screen
    public void LoadSceneWithLoadingScreen(string sceneName)
    {
        StartCoroutine(LoadAsync(sceneName));
    }

    private IEnumerator LoadAsync(string sceneName)
    {
        // Activate the loading screen
        loadingScreen.SetActive(true);

        // Start loading the scene asynchronously
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        // Prevent the scene from activating automatically
        operation.allowSceneActivation = false;

        // Start the three-dot animation on loading text
        StartCoroutine(UpdateLoadingText());

        // Keep track of the elapsed time to ensure a minimum load time
        float elapsedTime = 0f;

        // While loading, check for progress and update the text if needed
        while (!operation.isDone)
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

            // Update the elapsed time
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Once the scene is loaded, deactivate the loading screen
        loadingScreen.SetActive(false);
    }

    // Coroutine to update the loading text with dots animation
    private IEnumerator UpdateLoadingText()
    {
        while (true)
        {
            // Cycle through the three dot animation only while loading
            loadingText.text = "Loading.";
            yield return new WaitForSeconds(0.5f);
            loadingText.text = "Loading..";
            yield return new WaitForSeconds(0.5f);
            loadingText.text = "Loading...";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
