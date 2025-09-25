using UnityEngine;

public class RelationshipManager : MonoBehaviour
{
    public float relationship; // Relationship value with the NPC (0 - 100)

    // You can add some initial relationship values
    public float initialRelationship = 50f; // 50 represents a neutral relationship at the start

    void Start()
    {
        // Initialize the relationship with the NPC
        relationship = initialRelationship;
    }

    // Method to adjust the relationship based on decisions or actions
    public void AdjustRelationship(float amount)
    {
        relationship += amount;
        relationship = Mathf.Clamp(relationship, 0f, 100f); // Ensure the value stays between 0 and 100
    }

    // Method to get the current relationship value (can be used to display on UI)
    public float GetRelationship()
    {
        return relationship;
    }
}
