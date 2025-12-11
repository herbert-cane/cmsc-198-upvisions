using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject menuUI; // Menu UI reference 
    public GameObject encyclopediaUI; // Encyclopedia UI reference
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Debug.Log("Start method called!");
        // Debug.Log("GameObject active: " + gameObject.activeInHierarchy);
        if (menuUI != null)
        {
            menuUI.SetActive(false);
        }
        else
        {
            Debug.LogError("menuUI is not assigned!");
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) // Check if the Tab key is pressed
        {
            if (!menuUI.activeSelf && PauseController.isGamePaused) return; // Prevent opening menu if game is paused
            ToggleMenu(); // Toggle the menu UI
            PauseController.setPause(menuUI.activeSelf); // Update the pause state based on menu visibility 
        }

        // Toggle Encyclopedia with 'J'
        if (Input.GetKeyDown(KeyCode.J)) 
        {
            TogglePanel(encyclopediaUI);
        }
    }

    // Method to toggle the menu UI
    void ToggleMenu()
    {
        if (menuUI != null)
        {
            bool newState = !menuUI.activeSelf;
            menuUI.SetActive(newState);
        }
    }

    void TogglePanel(GameObject panel)
    {
        if (panel != null)
        {
            bool newState = !panel.activeSelf;
            panel.SetActive(newState);
            
            // Reuse your existing Pause logic!
            PauseController.setPause(newState); 
        }
    }
}
