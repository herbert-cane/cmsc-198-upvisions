using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static bool isInteracting = false; // Static property to track if the player is interacting

    public static void StartInteraction()
    {
        isInteracting = true; // Set interacting to true when an interaction starts (e.g., NPC dialogue)
    }

    public static void EndInteraction()
    {
        isInteracting = false; // Set interacting to false when the interaction ends
    }
}
