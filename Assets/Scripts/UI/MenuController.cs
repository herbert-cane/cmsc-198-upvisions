using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject menuUI; // Reference to the menu UI GameObject
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Start method called!");
        Debug.Log("GameObject active: " + gameObject.activeInHierarchy);
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
            ToggleMenu(); // Toggle the menu UI
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
}
