using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueController : MonoBehaviour
{
    public static DialogueController Instance { get; private set; } // Singleton instance
    public GameObject dialoguePanel; // UI panel for dialogue
    public TMP_Text dialogueText, nameText; // UI text components for dialogue and name
    public Image portraitImage; // UI image component for portrait  
    public Transform choicesContainer; // Container for dialogue choices
    public GameObject choiceButtonPrefab; // Prefab for choice buttons

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
        }
    }

    public void ShowDialoguePanel(bool show)
    {
        dialoguePanel.SetActive(show);
    }

    public void SetNPCInfo(string npcName, Sprite npcPortrait)
    {
        nameText.SetText(npcName);
        portraitImage.sprite = npcPortrait;
    }

    public void SetDialogueText(string text)
    {
        dialogueText.text = text;
    }

    public void ClearChoices()
    {
        foreach (Transform child in choicesContainer)
        {
            Destroy(child.gameObject);
        }
    }

    public GameObject CreateChoiceButton(string choiceText, UnityEngine.Events.UnityAction onClick)
    {
        GameObject choiceButtonObj = Instantiate(choiceButtonPrefab, choicesContainer);
        choiceButtonObj.GetComponentInChildren<TMP_Text>().text = choiceText;
        choiceButtonObj.GetComponent<Button>().onClick.AddListener(onClick);

        Debug.Log("Choice button created with text: " + choiceText);
        
        return choiceButtonObj;
    }
}
