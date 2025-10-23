using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EncyclopediaManager : MonoBehaviour
{
    // Drag all your entry assets here in the Inspector
    public List<EncyclopediaEntry> allEntries; 
    
    // Drag your UI panel references here
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

    // Call this from your category tab buttons
    public void PopulateEntryList(string entryType)
    {
        // ... (clear existing buttons in entryListContainer) ...
        
        foreach (EncyclopediaEntry entry in allEntries)
        {
            // Check if the entry is the type we want
            if (entry.GetType().Name == entryType)
            {
                GameObject buttonObj = Instantiate(entryListButtonPrefab, entryListContainer);
                // ... (set button text to entry.entryName) ...
                
                // Add a listener to the button
                buttonObj.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
                {
                    DisplayEntry(entry.entryID);
                });
            }
        }
    }

    // This is the core function
    public void DisplayEntry(string entryID)
    {
        if (!entryDatabase.ContainsKey(entryID))
        {
            Debug.LogError("Encyclopedia entry not found: " + entryID);
            return;
        }

        EncyclopediaEntry entry = entryDatabase[entryID];

        // 1. Populate Base Info
        titleText.text = entry.entryName;
        iconImage.sprite = entry.icon;
        descriptionText.text = entry.description; // TMP will parse the <link> tags!

        // 2. Populate Specialized Info (hide/show panels)
        statsPanel.SetActive(false);

        if (entry is ItemEntry item)
        {
            statsPanel.SetActive(true);
            // ... (set stats text for cost, type, etc.) ...
        }
        else if (entry is PersonalityEntry person)
        {
            statsPanel.SetActive(true);
            // ... (set stats text for role, knowledge tiers, etc.) ...
        }
        else if (entry is ProcessEntry process)
        {
            statsPanel.SetActive(true);
            // ... (set stats text for steps, mastery tip, etc.) ...
        }
        else if (entry is LocationEntry location)
        {
            statsPanel.SetActive(true);
            // ... (set stats text for address, hours, etc.) ...
        }
        else if (entry is CultureEntry culture)
        {
            statsPanel.SetActive(true);
            // ... (set stats text for traditions, festivals, etc.) ...
        }
    }
}