// using UnityEngine;
// using UnityEngine.UI;

// public class QuestManager : MonoBehaviour
// {
//     public static QuestManager Instance;

//     public GameObject questPanel;        // The UI panel for displaying quests
//     public Text questTitleText;          // UI Text for quest title
//     public Text questDescriptionText;    // UI Text for quest description
//     public Button acceptQuestButton;     // Button to accept the quest
//     public Button declineQuestButton;    // Button to decline the quest

//     private NPC currentNPC;              // The current NPC offering the quest

//     private void Awake()
//     {
//         if (Instance == null)
//             Instance = this;
//         else
//             Destroy(gameObject);
//     }

//     // Show the quest options after dialogue ends
//     public void ShowQuestOptions(Quest quest, NPC npc)
//     {
//         currentNPC = npc; // Store the NPC offering the quest
//         questPanel.SetActive(true); // Show the quest panel
//         questTitleText.text = quest.questTitle; // Display the quest title
//         questDescriptionText.text = quest.questDescription; // Display the quest description

//         // Add listener to accept quest
//         acceptQuestButton.onClick.AddListener(AcceptQuest);
//         declineQuestButton.onClick.AddListener(DeclineQuest);
//     }

//     // When the player accepts the quest
//     private void AcceptQuest()
//     {
//         currentNPC.AcceptQuest();  // Mark quest as accepted in NPC script
//         questPanel.SetActive(false);  // Hide quest UI
//         Debug.Log("Quest Accepted: " + currentNPC.quest.questTitle);  // Debug log
//     }

//     // When the player declines the quest
//     private void DeclineQuest()
//     {
//         questPanel.SetActive(false);  // Hide quest UI
//         Debug.Log("Quest Declined");
//     }

//     // Method to start the quest once accepted
//     public void StartQuest(Quest quest)
//     {
//         // You can add quest start logic here (tracking progress, rewards, etc.)
//         Debug.Log("Quest Started: " + quest.questTitle);
//     }
// }