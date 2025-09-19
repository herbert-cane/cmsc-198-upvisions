using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRadius = 2f; // The radius for the player to interact with the NPC
    public LayerMask npcLayer;           // The layer that contains the NPCs

    private NPC currentNPC;              // Reference to the currently interacted NPC

    void Update()
    {
        CheckForNPCInteraction();        // Continuously check for NPC interaction

        if (Input.GetKeyDown(KeyCode.E) && currentNPC != null) // If 'E' is pressed and an NPC is detected
        {
            InteractWithNPC();
        }
    }

    // Check if the player is within range of any NPC
    void CheckForNPCInteraction()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, interactionRadius, npcLayer);
        if (hitColliders.Length > 0)
        {
            // Get the first NPC in range
            currentNPC = hitColliders[0].GetComponent<NPC>();
        }
        else
        {
            currentNPC = null; // No NPC in range
        }
    }

    // Handle the interaction with the NPC
    void InteractWithNPC()
    {
        if (currentNPC != null)
        {
            Debug.Log("Interacting with NPC: " + currentNPC.name);
            currentNPC.StartDialogue(); // Start dialogue when interacting
            if (currentNPC.quest != null) // If the NPC has a quest
            {
                currentNPC.StartQuest(); // Start the quest
            }
        }
    }

    // Optionally: Draw a radius to visualize the interaction area in the scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}