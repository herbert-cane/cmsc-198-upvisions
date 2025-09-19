using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;  // Singleton

    public GameObject dialoguePanel;          // The UI panel containing the dialogue UI
    public TextMeshProUGUI npcNameText;                  // UI Text for NPC name
    public TextMeshProUGUI dialogueText;                 // UI Text for dialogue line
    public Image npcImage;                    // UI Image for NPC sprite
    public Image dialogueBackground;          // Image background for dialogue box
    public float typingSpeed = 0.05f;         // Speed of the typing effect

    private int currentLine = 0;              // Current dialogue line
    private NPC currentNPC;                   // Reference to the current NPC

    private void Awake()
    {
        // Singleton pattern to ensure only one instance
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void StartDialogue(Dialogue dialogue)
    {
        dialoguePanel.SetActive(true);  // Show the dialogue panel
        npcNameText.text = dialogue.npcName;   // Set NPC name
        npcImage.sprite = dialogue.npcSprite;  // Set NPC sprite

        // Ensure the NPC reference is correctly assigned
        currentNPC = dialogue.npcSprite.GetComponentInParent<NPC>();  // If the NPC sprite is a child of the NPC object
        Debug.Log("NPC assigned: " + (currentNPC != null ? currentNPC.name : "None"));
        
        StartCoroutine(TypeDialogue(dialogue.dialogueLines));  // Start typing dialogue
    }


    // Coroutine to simulate typing
    private IEnumerator TypeDialogue(System.Collections.Generic.List<string> dialogueLines)
    {
        foreach (string line in dialogueLines)
        {
            dialogueText.text = "";  // Clear the text field before typing the next line
            foreach (char letter in line)
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }

            // Wait for user input to continue to the next line
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }

        // After the dialogue ends, show quest options (if the NPC has a quest)
        if (currentNPC != null)
        {
            currentNPC.ShowQuestOptions();
        }

        EndDialogue();
    }

    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);  // Hide the dialogue panel
    }
}