using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ToggleVisibility : MonoBehaviour
{
    public static ToggleVisibility Instance { get; private set; } // Singleton instance
    [Header("UI Panels")]
    [SerializeField] private GameObject expandBigPhone; // The big phone UI panel
    [SerializeField] private GameObject hideSmallPhone; // The small phone UI panel (button to click)
    [SerializeField] private GameObject buttonToClick; // The button that triggers the toggle

    [Header("Objects to Toggle")]
    public List<GameObject> objectsToToggle;  // List of other objects to toggle visibility

    private AudioSource audioSource; // AudioSource for sound effects

        void Awake()
    {
        // Ensure the instance is set only once
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);  // Ensure only one instance exists
        }
    }
    private void Start()
    {
        // Ensure the small phone UI is visible and big phone UI is hidden initially
        expandBigPhone.SetActive(false); // Hide the big phone UI initially
        hideSmallPhone.SetActive(true); // Show the small phone UI (button) initially

        // Add an onClick listener to the button
        if (buttonToClick != null)
        {
            buttonToClick.GetComponent<Button>().onClick.AddListener(TogglePanelVisibility);
        }

        // Initialize audio source (you can add this to toggle sound for the panels if needed)
        audioSource = GameObject.FindWithTag("SFX")?.GetComponent<AudioSource>();
    }

    // Method to toggle visibility of the panels and other objects
    private void TogglePanelVisibility()
    {
        // Toggle visibility of the phone UI panels
        expandBigPhone.SetActive(!expandBigPhone.activeSelf);  // Toggle big phone UI

        // Pause the game when the big phone UI is shown
        if (expandBigPhone.activeSelf)
        {
            PauseController.setPause(true);  // Pause game
            PlaySFX();  // Play sound effect for toggling
        }
        else
        {
            PauseController.setPause(false); // Unpause game
        }

        // Toggle other objects in the list
        foreach (GameObject obj in objectsToToggle)
        {
            if (obj != null)
            {
                obj.SetActive(!obj.activeSelf);  // Toggle visibility of each object in the list
            }
        }
    }

    // Method to play a sound effect when toggling panels
    private void PlaySFX()
    {
        if (audioSource != null)
        {
            // Example: play a toggle sound, can customize as needed
            audioSource.PlayOneShot(Resources.Load<AudioClip>("ToggleSound")); 
        }
    }

    // Update is called once per frame to listen for key presses or other conditions
    private void Update()
    {
        // Check if the period (.) key is pressed to toggle the objects (like in original ToggleVisibility)
        if (Input.GetKeyDown(KeyCode.Period))
        {
            ToggleObjects();
        }

        // Check if the "T" key is pressed to revert the changes
        if (Input.GetKeyDown(KeyCode.T))
        {
            RevertPanelVisibility();
        }
    }

    // This method will toggle the visibility of objects when the "." key is pressed
    public void ToggleObjects()
    {
        foreach (GameObject obj in objectsToToggle)
        {
            if (obj != null)
            {
                obj.SetActive(!obj.activeSelf);  // Toggle visibility of each object in the list
            }
        }
    }

    // Method to revert the panels (this could be for resetting visibility)
    private void RevertPanelVisibility()
    {
        // Revert back to the initial UI state
        expandBigPhone.SetActive(false);  // Hide big phone
        hideSmallPhone.SetActive(true);   // Show small phone (button)

        // Unpause the game when the big phone UI is hidden
        PauseController.setPause(false);

        // Optionally: Revert the visibility of other objects if needed
        foreach (GameObject obj in objectsToToggle)
        {
            obj.SetActive(true);  // Set all toggled objects to be visible
        }
    }
}
