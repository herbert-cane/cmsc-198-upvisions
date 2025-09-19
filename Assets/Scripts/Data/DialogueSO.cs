using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "NPC/Dialogue")]
public class DialogueSO : ScriptableObject
{
    public string npcName;             // Name of the NPC
    public Sprite npcSprite;           // NPC Sprite
    public List<string> dialogueLines; // List of Dialogue lines
}