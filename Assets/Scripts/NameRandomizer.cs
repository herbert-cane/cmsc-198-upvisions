using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class NameRandomizer : MonoBehaviour
{
    public TMP_InputField nameText; // Reference to the TMP_InputField component (Input Field GameObject)
    public Button randomizerButton; // Reference to the Randomizer Button

    private List<string> firstNames = new List<string>
    {
        "Angela", "Chikki", "Christel", "Chrystie", "Claire", "Clairene", "Cloyd", "Cynel", 
        "Dexel", "Diana", "Dominique", "Eusef", "Evan", "Frances", "Francesca", "Francis", 
        "Gregorio", "Hannah", "Ianny", "Jaden", "Jakob", "Jason", "Jayvee", "John", "John", 
        "Julia", "Kolai", "Kristan", "Krizzian", "Leopoldo", "Liwayen", "Ma.", "Mark", "Mikhaela", 
        "Nina", "Philip", "Rafael", "Raia", "RJ", "Shan", "Sharee", "Shea", "Sophia", "Vincent", 
        "Vincent", "Walton", "Zion", "Zyann", "Lyell"
    };

    void Start()
    {
        // Make sure the button is assigned in the inspector
        if (randomizerButton != null)
        {
            randomizerButton.onClick.AddListener(ChangeNameText);
        }
        else
        {
            Debug.LogError("RandomizerButton is not assigned!");
        }
    }

    // This method is public so it can be accessed by the Button in the inspector
    public void ChangeNameText()
    {
        // Generate a random index and set the text to a random name
        int randomIndex = Random.Range(0, firstNames.Count);
        nameText.text = firstNames[randomIndex];
    }
}