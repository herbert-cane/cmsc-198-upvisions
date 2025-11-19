using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro; // Make sure to add this

public class SceneLoader : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider progressBar;
    public TextMeshProUGUI progressText;

    void Start()
    {
        // Check if we have a scene name to load
        if (string.IsNullOrEmpty(SceneData.sceneToLoad))
        {
            Debug.LogError("No scene to load! Returning to Main Menu.");
            SceneManager.LoadScene("MainMenu");
            return;
        }

        // Start the background loading process
        StartCoroutine(LoadSceneAsync(SceneData.sceneToLoad));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        // This is the operation that will load the scene in the background
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        // This prevents the scene from activating as soon as it's 90% done
        operation.allowSceneActivation = false; 

        // Loop until the scene is *almost* done loading
        while (operation.progress < 0.9f)
        {
            // Update the progress bar and text
            // operation.progress goes from 0 to 0.9. We remap it to 0-1.
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            
            progressBar.value = progress;
            progressText.text = (progress * 100f).ToString("F0") + "%";

            yield return null; // Wait for the next frame
        }

        // --- Loading is 90% complete ---
        // At this point, the scene is loaded in memory but not "activated."
        
        // You can add a "Press any key to continue" message here
        // or just activate it immediately.
        
        // Show 100% on the UI
        progressBar.value = 1.0f;
        progressText.text = "100%";
        // (Optional: "Loading complete. Press any key...")

        // (Optional: Wait for user input)
        // while (!Input.anyKeyDown)
        // {
        //     yield return null;
        // }

        // --- Activate the new scene ---
        // This will unload the LoadingScreen and show the new scene
        operation.allowSceneActivation = true;
    }
}