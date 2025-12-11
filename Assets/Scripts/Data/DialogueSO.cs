using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewNPCDialogue", menuName = "NPC Dialogue")]
public class DialogueSO : ScriptableObject
{
    public string npcName;                      // Name of the NPC
    public Sprite npcPortrait;                  // NPC Portrait
    public string[] dialogueLines;              // Array of dialogue lines
    public bool[] autoProgressLines;            // Array indicating which lines auto-progress
    public bool[] endDialogueLines;             // Array indicating which lines end the dialogue
    public float autoProgressDelay = 1.5f;      // Delay before auto-progressing
    public float typingSpeed = 0.05f;           // Speed of the typing effect
    public AudioClip voiceSound;                // Sound played during typing
    public float voicePitch = 1f;               // Pitch of the voice sound

    public DialogueChoice[] dialogueChoices;    // Array of dialogue choices
    public NPCType npcType;                     // NPC Type (e.g., Counselor, Shopkeeper)
}

[System.Serializable]
public class DialogueChoice
{
    public int dialogueIndex;                   // Index of the dialogue line
    public string[] choices;                    // Array of choices for the player
    public int[] nextDialogueIndexes;           // Corresponding next dialogue indexes for each choice
    public string[] choiceActions;              // e.g. ["StartCounseling", "Leave"]
}
