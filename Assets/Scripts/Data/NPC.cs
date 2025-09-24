using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using System.Collections;
public class NPC : MonoBehaviour, IInteractable
{
    public DialogueSO dialogue;   // NPC's dialogue
    public GameObject dialoguePanel; // UI panel for dialogue
    public TMP_Text dialogueText, nameText; // UI text components for dialogue and name
    public Image portraitImage; // UI image component for portrait  
    private int dialogueIndex; // Current index in the dialogue
    private bool isTyping, isDialogueActive; // Flags for typing effect and dialogue state
    private AudioSource audioSource; // AudioSource for playing sound effects

    // Method to check if the NPC can be interacted with
    public bool CanInteract()
    {
        return !isDialogueActive;
    }

    void Awake()
    {
        GameObject sfxObject = GameObject.FindGameObjectWithTag("SFX");
        if (sfxObject != null)
        {
            audioSource = sfxObject.GetComponent<AudioSource>();
        }
    }
    void Update()
    {
        // Only check for key press if dialogue is active and the game is paused
        if (isDialogueActive && !isTyping)
        {
            if (Input.GetMouseButtonDown(0)) // Left mouse button to advance dialogue
            {
                NextLine();
            }
        }
    }
    // Method to handle interaction with the NPC
    public void Interact()
    {
        if (dialogue == null || (PauseController.isGamePaused && !isDialogueActive))
            return;

        if (isDialogueActive)
        {
            // Only go to the next line if it's not currently typing
            if (!isTyping)
            {
                NextLine();
            }
        }
        else
        {
            StartDialogue();
        }
    }
    // Method to start the dialogue with the player
    public void StartDialogue()
    {
        isDialogueActive = true;
        dialogueIndex = 0;

        // Safely set the NPC's name in the UI (TMP_Text). Use the dialogue name if available.
        nameText.SetText(dialogue.npcName);
        portraitImage.sprite = dialogue.npcPortrait;

        dialoguePanel.SetActive(true); // Show the dialogue panel
        PauseController.setPause(true); // Pause the game

        StartCoroutine(TypeLine()); // Start typing the first line

    }

    void NextLine()
    {
        if (isTyping)
        {
            // If typing, stop the typing coroutine and show the full line immediately
            StopAllCoroutines();
            dialogueText.text = dialogue.dialogueLines[dialogueIndex];
            isTyping = false;
        }
        else if (++dialogueIndex < dialogue.dialogueLines.Length)
        {
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    public void EndDialogue()
    {
        StopAllCoroutines();
        isDialogueActive = false;
        dialogueText.text = "";
        dialoguePanel.SetActive(false); // Hide the dialogue panel
        PauseController.setPause(false); // Unpause the game
    }
    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.text = ""; // Clear the text at the start of typing

        
        foreach (char letter in dialogue.dialogueLines[dialogueIndex])
        {
            dialogueText.text += letter;
            if (audioSource != null && dialogue.voiceSound != null)
            {
                audioSource.pitch = dialogue.voicePitch;
                audioSource.PlayOneShot(dialogue.voiceSound);
            }

            yield return new WaitForSeconds(dialogue.typingSpeed);
        }
        isTyping = false;

        // If auto-progress is enabled, move to the next line after the delay
        if(dialogue.autoProgressLines.Length > dialogueIndex && dialogue.autoProgressLines[dialogueIndex])
        {
            yield return new WaitForSeconds(dialogue.autoProgressDelay);
            NextLine();
        }
    }
}