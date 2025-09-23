using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Scene Names")]
    public string characterSelectScene = "CharacterSelect";  // Name of your character selection scene
    public string gameScene = "GameScene";                    // If skipping selection

    [Header("UI Panels")]
    public GameObject optionsPanel;  // Reference to the Options Panel

    private bool isOptionsPanelActive = false; // Track if the Options Panel is open or not

    void Start()
    {
        // Ensure the options panel is hidden when the game starts
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(false);
        }
    }

    public void NewGame()
    {
        SceneManager.LoadScene(characterSelectScene);
    }

    public void LoadGame()
    {
        Debug.Log("Load Game clicked");
        // TODO: Add your load logic here later
    }

    public void Options()
    {
        Debug.Log("Options clicked");

        // Toggle the options panel visibility
        if (optionsPanel != null)
        {
            isOptionsPanelActive = !isOptionsPanelActive; // Toggle state
            optionsPanel.SetActive(isOptionsPanelActive); // Show or hide based on state
        }
        else
        {
            Debug.LogWarning("Options Panel is not assigned!");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit clicked (won’t quit in editor)");
    }
}
