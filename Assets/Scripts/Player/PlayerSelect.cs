using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CharacterSelect : MonoBehaviour
{
    [Header("Player Characters")]
    public GameObject maleCharacter;
    public GameObject femaleCharacter;

    [Header("Character Glows")]
    public GameObject maleGlow;
    public GameObject femaleGlow;

    [Header("Organization Cards")]
    public Button[] cards;
    public GameObject[] cardGlows; // Glow Image child of each card

    [Header("Name Input")]
    public TMP_InputField nameInput;

    [Header("Pulse Settings")]
    public float pulseSpeed = 2f;
    public float pulseAmount = 0.1f;

    private bool isMale = true;
    private int selectedCardIndex = -1;

    // List of currently active glows to pulse
    private List<GameObject> activeGlows = new List<GameObject>();
    private float pulseTimer = 0f;

    void Start()
    {
        // Ensure characters are active
        maleCharacter.SetActive(true);
        femaleCharacter.SetActive(true);
        UpdateCharacterSelection();

        // Assign click listeners to cards
        for(int i = 0; i < cards.Length; i++)
        {
            int index = i; // local copy for closure
            cards[i].onClick.AddListener(() => SelectCard(index));
        }

        UpdateCardSelection();
        UpdateActiveGlows();
    }

    void Update()
    {
        // Update pulse for all active glows
        pulseTimer += Time.deltaTime * pulseSpeed;
        float scale = 1f + Mathf.Sin(pulseTimer) * pulseAmount;

        foreach (GameObject glow in activeGlows)
        {
            if(glow != null && glow.activeSelf)
                glow.transform.localScale = Vector3.one * scale;
        }
    }

    // ----------------- Character Selection -----------------
    public void SelectCharacter(bool selectMale)
    {
        isMale = selectMale;
        UpdateCharacterSelection();
        UpdateActiveGlows();
        Debug.Log("Selected Character: " + (isMale ? "Male" : "Female"));
    }

    private void UpdateCharacterSelection()
    {
        // Activate glows
        maleGlow.SetActive(isMale);
        femaleGlow.SetActive(!isMale);

        // Dim unselected
        SetDim(maleCharacter, !isMale);
        SetDim(femaleCharacter, isMale);
    }

    // ----------------- Card Selection -----------------
    public void SelectCard(int index)
    {
        selectedCardIndex = index;
        UpdateCardSelection();
        UpdateActiveGlows();
        Debug.Log("Selected Card: " + cards[index].name);
    }

    private void UpdateCardSelection()
    {
        for(int i = 0; i < cards.Length; i++)
        {
            bool isSelected = (i == selectedCardIndex);

            // Activate glow
            cardGlows[i].SetActive(isSelected);

            // Dim unselected
            SetDim(cards[i].gameObject, !isSelected);
        }
    }

    // ----------------- Name Input & Confirm -----------------
    public void ConfirmSelection()
    {
        string playerName = nameInput.text;
        Debug.Log("Chosen Name: " + playerName +
                  ", Gender: " + (isMale ? "Male" : "Female") +
                  ", Card: " + (selectedCardIndex >= 0 ? cards[selectedCardIndex].name : "None"));

        // TODO: Save selection and proceed to next scene
    }

    // ----------------- Utility -----------------
    private void SetDim(GameObject obj, bool dim)
    {
        Image img = obj.GetComponent<Image>();
        if(img != null)
            img.color = dim ? new Color(1,1,1,0.5f) : Color.white;
    }

    private void UpdateActiveGlows()
    {
        activeGlows.Clear();

        // Add selected character glow
        if(isMale) activeGlows.Add(maleGlow);
        else activeGlows.Add(femaleGlow);

        // Add selected card glow
        if(selectedCardIndex >= 0 && cardGlows.Length > selectedCardIndex)
            activeGlows.Add(cardGlows[selectedCardIndex]);
    }
}