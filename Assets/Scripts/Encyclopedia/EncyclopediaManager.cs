using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EncyclopediaManager : MonoBehaviour
{
    // Drag all entry assets here in the Inspector
    public List<EncyclopediaEntry> allEntries; 
    
    // Drag UI panel references here
    public GameObject encyclopediaPanel;
    public Transform entryListContainer;
    public GameObject entryListButtonPrefab; // A simple button prefab
    
    // Drag the UI elements from the Right_Panel here
    public TMPro.TextMeshProUGUI titleText;
    public UnityEngine.UI.Image iconImage;
    public TMPro.TextMeshProUGUI descriptionText;
    public GameObject statsPanel; // The parent panel for stats
    
    // A dictionary for fast lookups by ID
    private Dictionary<string, EncyclopediaEntry> entryDatabase = new Dictionary<string, EncyclopediaEntry>();

    void Awake()
    {
        // Load all entries into the fast-lookup dictionary
        foreach (EncyclopediaEntry entry in allEntries)
        {
            if (!entryDatabase.ContainsKey(entry.entryID))
            {
                entryDatabase.Add(entry.entryID, entry);
            }
        }
    }

    public void OpenEncyclopedia()
    {
        encyclopediaPanel.SetActive(true);
        // Default to showing the 'Items' category first
        PopulateEntryList("ItemEntry"); 
    }

// Call this from category tab buttons
    public void PopulateEntryList(string entryType)
    {
        // --- FIX 1: Robust List Clearing ---
        // We iterate backwards or use a specific loop to ensure everything is destroyed properly
        foreach (Transform child in entryListContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (EncyclopediaEntry entry in allEntries)
        {
            // Check if the entry is the type we want
            if (entry.GetType().Name == entryType)
            {
                GameObject buttonObj = Instantiate(entryListButtonPrefab, entryListContainer);
                
                // --- FIX 2: Correct Text Component ---
                // You are using TextMeshPro, so we must look for TextMeshProUGUI, not Text!
                TMPro.TextMeshProUGUI btnText = buttonObj.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                if (btnText != null)
                {
                    btnText.text = entry.entryName;
                }
                else
                {
                    Debug.LogWarning("Button Prefab is missing a TextMeshProUGUI component!");
                }
                
                // Add a listener to the button
                buttonObj.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
                {
                    DisplayEntry(entry.entryID);
                });
            }
        }
    }

    // This is the core function to display an entry's details
public void DisplayEntry(string entryID)
    {
        if (!entryDatabase.ContainsKey(entryID))
        {
            Debug.LogError("Encyclopedia entry not found: " + entryID);
            return;
        }

        EncyclopediaEntry entry = entryDatabase[entryID];

        // --- FIX 3: Safety Checks for Null UI ---
        // This prevents the NullReferenceException if you forgot to drag something in
        if (titleText != null) titleText.text = entry.entryName;
        
        if (iconImage != null)
        {
            if (entry.icon != null)
            {
                iconImage.sprite = entry.icon;
                iconImage.gameObject.SetActive(true);
            }
            else
            {
                iconImage.gameObject.SetActive(false);
            }
        }

        if (statsPanel != null) statsPanel.SetActive(false);

        // Populate Description based on type
        // We check if descriptionText is assigned to avoid the crash
        if (descriptionText != null)
        {
            if (entry is LocationEntry location)
            {
                descriptionText.text = location.fullDescription;
            }
            else if (entry is CultureEntry culture)
            {
                descriptionText.text = culture.fullDescription;
            }
            else
            {
                // Default for Items, Personalities, Processes
                descriptionText.text = entry.initialDescription;
            }
        }
        else
        {
            Debug.LogError("Description Text is not assigned in the EncyclopediaManager Inspector!");
        }

        if (statsPanel != null)
        {
            // Re-enable stats panel only for types that need it
            if (entry is ItemEntry || entry is PersonalityEntry || entry is ProcessEntry || entry is LocationEntry)
            {
                statsPanel.SetActive(true);
            }
        }
    }
}