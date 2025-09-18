using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Scene Names")]
    public string characterSelectScene = "CharacterSelect";  // name of your character selection scene
    public string gameScene = "GameScene";                    // if skipping selection

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
        // TODO: Open an options UI panel
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit clicked (won’t quit in editor)");
    }
}
    