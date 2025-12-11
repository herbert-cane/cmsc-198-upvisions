using UnityEngine;

public class CounselingService : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private float stressReliefAmount = 30f;
    [SerializeField] private float sanityGainAmount = 10f;
    
    // If you want it to cost "Social Energy" or money, add that here.
    // For a school guidance counselor, it's usually free but might take TIME.
    [SerializeField] private float timeCostMinutes = 30f; 

    [Header("Dialogue References")]
    [SerializeField] private DialogueSO welcomeDialogue;
    [SerializeField] private DialogueSO successDialogue;
    [SerializeField] private DialogueSO alreadyCalmDialogue;

    // References
    private Player player;
    private TimeManager timeManager; // From your Time System

    private void Start()
    {
        player = FindFirstObjectByType<Player>();
        timeManager = FindFirstObjectByType<TimeManager>();
    }

    // Call this function when the player interacts with the Counselor NPC
    public void StartCounselingSession()
    {
        // 1. Check if player actually needs help
        if (player.playerStats.stress <= 10)
        {
            // Player is already calm
            StartDialogue(alreadyCalmDialogue);
        }
        else
        {
            // Player needs help -> Show welcome/confirmation dialogue
            // You can modify your DialogueManager to call "PerformCounseling" 
            // after a specific choice, OR just do it automatically for now.
            PerformCounseling(); 
        }
    }

    // The actual mechanics
    public void PerformCounseling()
    {
        if (player == null) return;

        // 1. Apply Stats Changes
        player.playerStats.stress = Mathf.Max(0, player.playerStats.stress - stressReliefAmount);
        player.playerStats.sanity = Mathf.Min(100, player.playerStats.sanity + sanityGainAmount);

        // 2. Consume Time (Advance the clock)
        if (timeManager != null)
        {
            // Assuming your TimeManager has a method to skip time
            // If not, you can access the variables directly or add a method:
            // timeManager.SkipTime(timeCostMinutes); 
        }

        Debug.Log("Counseling Complete. Stress reduced.");

        // 3. Play Success Dialogue
        StartDialogue(successDialogue);
    }

    private void StartDialogue(DialogueSO dialogue)
    {
        // This connects to your existing Dialogue System
        GameEventsManager.instance.dialogueEvents.EnterDialogue(dialogue.name); 
        // Note: You might need to adjust this depending on how your DialogueManager 
        // finds dialogue (by name, or by passing the SO directly).
    }
}