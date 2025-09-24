using UnityEngine;

public class TogglePanel : MonoBehaviour
{
    [SerializeField] private GameObject expandBigPhone; // The big phone UI panel
    [SerializeField] private GameObject hideSmallPhone; // The small phone UI panel (button to click)
    [SerializeField] private GameObject buttonToClick; // The button that triggers the toggle

    private void Start()
    {
        // Ensure the small phone UI is visible and big phone UI is hidden initially
        expandBigPhone.SetActive(false); // Hide the big phone UI initially
        hideSmallPhone.SetActive(true); // Show the small phone UI (the button) initially

        // Add an onClick listener to the button
        if (buttonToClick != null)
        {
            buttonToClick.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(TogglePanelVisibility);
        }
    }

    // This method toggles the visibility of the panels when the button is clicked
    private void TogglePanelVisibility()
    {
        // Show the big phone UI and hide the small phone UI
        expandBigPhone.SetActive(true); // Show big phone
        hideSmallPhone.SetActive(false);  // Hide small phone (button)
        Debug.Log("Big phone UI shown, small phone UI hidden.");

        // Pause the game when the big phone UI is shown
        PauseController.setPause(true);
    }

    // Update is called once per frame to listen for key presses
    private void Update()
    {
        // Check if the "T" key is pressed to revert the changes
        if (Input.GetKeyDown(KeyCode.T))
        {
            RevertPanelVisibility();
        }
    }

    // This method will revert the panels when the "T" key is pressed
    private void RevertPanelVisibility()
    {
        // Show the small phone UI and hide the big phone UI
        expandBigPhone.SetActive(false);  // Hide big phone
        hideSmallPhone.SetActive(true); // Show small phone (button)
        Debug.Log("Big phone UI hidden, small phone UI shown.");

        // Resume the game when the big phone UI is hidden
        PauseController.setPause(false);
    }
}