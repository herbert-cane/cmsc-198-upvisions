using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement; // <-- ADDED THIS

public class CharacterSelect : MonoBehaviour
{
    [Header("Scene Configuration")]
    public string gameSceneName = "UI_Development_Ace"; // <-- The name of the scene to load next

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
        for (int i = 0; i < cards.Length; i++)
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
            if (glow != null && glow.activeSelf)
                glow.transform.localScale = Vector3.one * scale;
        }
    }

    // ----------------- Character Selection -----------------
    public void SelectCharacter(bool selectMale)
    {
        isMale = selectMale;
        UpdateCharacterSelection();
        UpdateActiveGlows();
        // Removed Debug.Log to keep console clean
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
    }

    private void UpdateCardSelection()
    {
        for (int i = 0; i < cards.Length; i++)
        {
            bool isSelected = (i == selectedCardIndex);

            // Activate glow
            if (i < cardGlows.Length && cardGlows[i] != null)
            {
                cardGlows[i].SetActive(isSelected);
            }

            // Dim unselected
            SetDim(cards[i].gameObject, !isSelected);
        }
    }

    // ----------------- Name Input & Confirm -----------------
    public void ConfirmSelection()
    {
        // 1. Validate Name
        if (string.IsNullOrEmpty(nameInput.text))
        {
            Debug.LogWarning("Please enter a name!");
            // Optional: Add a UI effect here to shake the input field
            return;
        }

        // 2. Validate Card Selection
        if (selectedCardIndex == -1)
        {
            Debug.LogWarning("Please select an Organization Card!");
            return;
        }

        // 3. Save Data to SceneData
        SceneData.playerName = nameInput.text;

        // Convert the bool to the String ID your GameInitializer expects
        SceneData.selectedAvatarID = isMale ? "Boy" : "Girl"; 

        // Use the GameObject's name as the ID (Make sure buttons are named "UGY", "BCP", etc.)
        SceneData.selectedCourseID = cards[selectedCardIndex].name; 

        Debug.Log($"Saved: {SceneData.playerName}, {SceneData.selectedAvatarID}, {SceneData.selectedCourseID}");

        // 4. Set the next scene and load the loading screen
        SceneData.sceneToLoad = gameSceneName;
        SceneManager.LoadScene("LoadingScreen");
    }

    // ----------------- Utility -----------------
    private void SetDim(GameObject obj, bool dim)
    {
        Image img = obj.GetComponent<Image>();
        if (img != null)
            img.color = dim ? new Color(1, 1, 1, 0.5f) : Color.white;
    }

    private void UpdateActiveGlows()
    {
        activeGlows.Clear();

        // Add selected character glow
        if (isMale) activeGlows.Add(maleGlow);
        else activeGlows.Add(femaleGlow);

        // Add selected card glow
        if (selectedCardIndex >= 0 && selectedCardIndex < cardGlows.Length)
            activeGlows.Add(cardGlows[selectedCardIndex]);
    }
}