using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

[CreateAssetMenu(fileName = "NewNPCDialogue", menuName = "NPC Dialogue")]
public class DialogueSO : ScriptableObject
{
    public string npcName;                      // Name of the NPC
    public Sprite npcPortrait;                  // NPC Portrait
    public string[] dialogueLines;              // Array of dialogue lines
    public bool[] autoProgressLines;            // Array indicating which lines auto-progress
    public float  autoProgressDelay = 1.5f;     // Delay before auto-progressing
    public float typingSpeed = 0.05f;           // Speed of the typing effect
    public AudioClip voiceSound;                // Sound played during typing
    public float voicePitch = 1f;                // Pitch of the voice sound

}