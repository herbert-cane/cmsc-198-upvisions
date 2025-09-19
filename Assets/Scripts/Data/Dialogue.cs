using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]  // Add this line to make it serializable
public class Dialogue
{
    public string npcName;           // Name of the NPC
    public Sprite npcSprite;         // Sprite of the NPC
    public List<string> dialogueLines;  // List of dialogue lines

    public Dialogue(string name, Sprite sprite, List<string> lines)
    {
        npcName = name;
        npcSprite = sprite;
        dialogueLines = lines;
    }
}
