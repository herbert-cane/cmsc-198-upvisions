using System.Collections;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    public DialogueSO dialogue;   // NPC's dialogue
    public NPCType npcType = NPCType.Default;  // NPC type (e.g., Counselor)
    private DialogueController dialogueUI;
    private int dialogueIndex;
    private bool isTyping, isDialogueActive;
    private AudioSource audioSource;

    private ToggleVisibility toggleVisibility;

    private void Start()
    {
        dialogueUI = DialogueController.Instance;
        toggleVisibility = ToggleVisibility.Instance;
    }

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
        if (isDialogueActive && !isTyping)
        {
            if (Input.GetKey(KeyCode.F)) // Press F to advance dialogue
            {
                NextLine();
            }
        }
    }

    public void Interact()
    {
        if (dialogue == null || PauseController.isGamePaused)
            return;

        if (isDialogueActive)
        {
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

    public void StartDialogue()
    {
        isDialogueActive = true;
        dialogueIndex = 0;

        dialogueUI.SetNPCInfo(dialogue.npcName, dialogue.npcPortrait);
        dialogueUI.ShowDialoguePanel(true);
        PauseController.setPause(true);

        if (toggleVisibility != null)
        {
            toggleVisibility.ToggleObjects();
        }

        DisplayCurrentLine();
    }

    void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueUI.SetDialogueText(dialogue.dialogueLines[dialogueIndex]);
            isTyping = false;
        }

        dialogueUI.ClearChoices();

        if (dialogue.endDialogueLines.Length > dialogueIndex && dialogue.endDialogueLines[dialogueIndex])
        {
            EndDialogue();
            return;
        }

        foreach (DialogueChoice choice in dialogue.dialogueChoices)
        {
            if (choice.dialogueIndex == dialogueIndex)
            {
                DisplayChoices(choice);
                return;
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

            // If the NPC is a counselor and the choice is about stress reduction
            if (npcType == NPCType.Counselor && choice.choices[i].Contains("Talk to the counselor about my stress"))
            {
                // Add a callback function to the choice button that triggers stress reduction only when clicked
                dialogueUI.CreateChoiceButton(choice.choices[i], () => OnTalkToCounselorClick(nextIndex));
            }
            else
            {
                dialogueUI.CreateChoiceButton(choice.choices[i], () => ChooseOption(nextIndex));
            }
        }
    }

    // This method will be called when the player clicks the "Talk to the counselor about my stress" button
    void OnTalkToCounselorClick(int nextIndex)
    {
        // Trigger the stress reduction
        FindFirstObjectByType<PlayerStatsTracker>().TalkToCounselor();

        // Proceed to the next dialogue line
        ChooseOption(nextIndex);
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

        if (toggleVisibility != null)
        {
            toggleVisibility.ToggleObjects();
        }

        PauseController.setPause(false);
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueUI.SetDialogueText("");

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

        if (dialogue.autoProgressLines.Length > dialogueIndex && dialogue.autoProgressLines[dialogueIndex])
        {
            yield return new WaitForSeconds(dialogue.autoProgressDelay);
            NextLine();
        }
    }
}