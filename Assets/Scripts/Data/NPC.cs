using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class NPC : MonoBehaviour, IInteractable
{
    public DialogueSO dialogue;   // NPC's dialogue
    private DialogueController dialogueUI; // Reference to the DialogueController
    private int dialogueIndex; // Current index in the dialogue
    private bool isTyping, isDialogueActive; // Flags for typing effect and dialogue state
    private AudioSource audioSource; // AudioSource for playing sound effects

    private ToggleVisibility toggleVisibility; // Reference to the ToggleVisibility script

    private void Start()
    {
        dialogueUI = DialogueController.Instance;

        // Initialize ToggleVisibility instance
        toggleVisibility = ToggleVisibility.Instance;
    }

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
            if (Input.GetKey(KeyCode.F)) // F key to advance dialogue
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
        dialogueUI.SetNPCInfo(dialogue.npcName, dialogue.npcPortrait);

        dialogueUI.ShowDialoguePanel(true);
        PauseController.setPause(true); // Pause the game

        // Toggle other objects (for example, hide the UI panels)
        if (toggleVisibility != null)
        {
            toggleVisibility.ToggleObjects(); // Toggle visibility of objects
        }

        DisplayCurrentLine();
    }

    void NextLine()
    {
        if (isTyping)
        {
            // If typing, stop the typing coroutine and show the full line immediately
            StopAllCoroutines();
            dialogueUI.SetDialogueText(dialogue.dialogueLines[dialogueIndex]);
            isTyping = false;
        }

        // Clear previous choices
        dialogueUI.ClearChoices();

        // Check for dialogue choices at the current index
        if (dialogue.endDialogueLines.Length > dialogueIndex && dialogue.endDialogueLines[dialogueIndex])
        {
            EndDialogue();
            return;
        }

        // Check if choices and display them
        foreach (DialogueChoice choice in dialogue.dialogueChoices)
        {
            if (choice.dialogueIndex == dialogueIndex)
            {
                // Display Choices
                DisplayChoices(choice);
                return; // Exit to wait for player choice
            }
        }

        if (++dialogueIndex < dialogue.dialogueLines.Length)
        {
            DisplayCurrentLine();
        }
        else
        {
            EndDialogue();
        }
    }

    void DisplayChoices(DialogueChoice choice)
    {
        for (int i = 0; i < choice.choices.Length; i++)
        {
            int nextIndex = choice.nextDialogueIndexes[i];
            dialogueUI.CreateChoiceButton(choice.choices[i], () => ChooseOption(nextIndex));
        }
    }

    void ChooseOption(int nextIndex)
    {
        dialogueIndex = nextIndex;
        dialogueUI.ClearChoices();
        DisplayCurrentLine();
    }

    void DisplayCurrentLine()
    {
        StopAllCoroutines();
        StartCoroutine(TypeLine());
    }

    public void EndDialogue()
    {
        StopAllCoroutines();
        isDialogueActive = false;
        dialogueUI.SetDialogueText("");
        dialogueUI.ShowDialoguePanel(false);

        // Toggle visibility of the UI objects when the dialogue ends
        if (toggleVisibility != null)
        {
            toggleVisibility.ToggleObjects(); // Toggle visibility of objects
        }

        PauseController.setPause(false); // Unpause the game
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueUI.SetDialogueText(""); // Clear the text at the start of typing

        foreach (char letter in dialogue.dialogueLines[dialogueIndex])
        {
            dialogueUI.SetDialogueText(dialogueUI.dialogueText.text + letter);
            if (audioSource != null && dialogue.voiceSound != null)
            {
                audioSource.pitch = dialogue.voicePitch;
                audioSource.PlayOneShot(dialogue.voiceSound);
            }

            yield return new WaitForSeconds(dialogue.typingSpeed);
        }
        isTyping = false;

        // If auto-progress is enabled, move to the next line after the delay
        if (dialogue.autoProgressLines.Length > dialogueIndex && dialogue.autoProgressLines[dialogueIndex])
        {
            yield return new WaitForSeconds(dialogue.autoProgressDelay);
            NextLine();
        }
    }
}