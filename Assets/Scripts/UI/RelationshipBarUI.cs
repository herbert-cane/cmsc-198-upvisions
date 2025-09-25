using UnityEngine;
using UnityEngine.UI;

public class RelationshipUI : MonoBehaviour
{
    public Image relationshipBar;  // The UI Image for the relationship meter
    public RelationshipManager relationshipManager;  // Reference to the RelationshipManager

    void Start()
    {
        // Make sure the relationshipManager is assigned (can be done in the inspector or here)
        if (relationshipManager == null)
        {
            relationshipManager = FindFirstObjectByType<RelationshipManager>();  // Find the RelationshipManager in the scene
        }
    }

    void Update()
    {
        // Update the relationship bar fill based on the player's relationship
        UpdateStatDisplay();
    }

    // Method to update the stat bar (image fill) based on the relationship
    private void UpdateStatDisplay()
    {
        // Set the fill amount based on the player's relationship value (0 to 1 range)
        relationshipBar.fillAmount = relationshipManager.GetRelationship() / 100f;
    }

    // Optional: You can add methods to update individual bars if needed
    public void UpdateStatBar(string statName)
    {
        switch (statName)
        {
            case "Relationship":
                relationshipBar.fillAmount = relationshipManager.GetRelationship() / 100f;
                break;
            default:
                Debug.LogWarning("Stat name not recognized.");
                break;
        }
    }
}