using UnityEngine;
using UnityEngine.UI;
using TMPro; // Needed for the Input Field
using UnityEngine.SceneManagement;

public class CharacterCreationManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField nameInputField; // Drag your "Enter Name" input field here

    [Header("Selection Feedback")]
    // Drag the Golden Circle Image GameObject here
    public GameObject avatarSelectionFrame; 
    // Drag the Frame that highlights the selected Card here
    public GameObject courseSelectionFrame; 

    [Header("Configuration")]
    public string gameSceneName = "GameProper"; // Your actual game scene

    // Private variables to hold current choices
    private string currentAvatarID;
    private string currentCourseID;
    
    // --- AVATAR SELECTION ---
    
    // Call this from the Buttons on the invisible overlay of your characters
    // Assign the button parameter as "Male" or "Female"
    public void SelectAvatar(string avatarID)
    {
        currentAvatarID = avatarID;
        Debug.Log("Avatar Selected: " + avatarID);

        // VISUALS: Move the Golden Circle to the clicked button
        // Note: This assumes the button clicked is the one passing the event
        GameObject clickedButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        if (clickedButton != null && avatarSelectionFrame != null)
        {
            avatarSelectionFrame.transform.position = clickedButton.transform.position;
        }
    }

    // --- COURSE SELECTION ---

    // Call this from the Card Buttons (UGY, ELEK, etc.)
    // Assign the parameter as "UGY", "BCP", etc.
    public void SelectCourse(string courseID)
    {
        currentCourseID = courseID;
        Debug.Log("Course Selected: " + courseID);

        // VISUALS: Move the Card Frame to the clicked card
        GameObject clickedButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        if (clickedButton != null && courseSelectionFrame != null)
        {
            courseSelectionFrame.transform.position = clickedButton.transform.position;
        }
    }

    // --- CONFIRM BUTTON ---

    public void ConfirmAndStart()
    {
        // 1. Validation: Did they type a name?
        if (string.IsNullOrEmpty(nameInputField.text))
        {
            Debug.LogWarning("Please enter a name!");
            return; // Don't start yet
        }
        
        // 2. Validation: Did they pick an avatar and course?
        if (string.IsNullOrEmpty(currentAvatarID) || string.IsNullOrEmpty(currentCourseID))
        {
            Debug.LogWarning("Please select an Avatar and a Course!");
            return;
        }

        // 3. Save ALL data to the Static Class
        SceneData.playerName = nameInputField.text;
        SceneData.selectedAvatarID = currentAvatarID;
        SceneData.selectedCourseID = currentCourseID;
        SceneData.sceneToLoad = gameSceneName;

        // 4. Load the Loading Screen
        SceneManager.LoadScene("LoadingScreen");
    }
}