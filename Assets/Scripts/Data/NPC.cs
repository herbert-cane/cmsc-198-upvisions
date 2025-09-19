using UnityEngine;
using System.Collections.Generic;
public class NPC : MonoBehaviour
{
    public Dialogue dialogue;   // NPC's dialogue
    public Quest quest;         // NPC's quest

    private bool hasAcceptedQuest = false; // Flag to track quest acceptance

    // Method to start the dialogue with the player
    public void StartDialogue()
    {
        DialogueManager.Instance.StartDialogue(dialogue);
    }

    // Method to show the quest options after dialogue
    public void ShowQuestOptions()
    {
        if (quest != null && !hasAcceptedQuest) // If the NPC has a quest and the player hasn't accepted it yet
        {
            QuestManager.Instance.ShowQuestOptions(quest, this); // Show the quest UI and allow acceptance
        }
    }

    // Method to handle the player accepting the quest
    public void AcceptQuest()
    {
        hasAcceptedQuest = true; // Set the quest as accepted
        QuestManager.Instance.StartQuest(quest); // Start the quest
        Debug.Log("Quest Accepted: " + quest.questTitle); // Debug log
    }

    // Method to start the quest with the player
    public void StartQuest()
    {
        if (quest != null)
        {
            QuestManager.Instance.StartQuest(quest);
            Debug.Log("Starting Quest: " + quest.questTitle);
        }
        else
        {
            Debug.LogWarning("This NPC has no quest assigned.");
        }
    }
}
