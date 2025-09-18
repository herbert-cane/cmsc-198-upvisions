using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUI : MonoBehaviour
{
    // References to the UI Sliders for stats display
    public Slider energySlider;
    public Slider sanitySlider;
    public Slider stressSlider;
    public Slider focusSlider;
    public Slider knowledgeSlider;
    public Slider socialLifeSlider;
    public Slider financesSlider;
    public Slider healthSlider;
    public Slider sleepSlider;
    public Slider motivationSlider;
    public Slider luckSlider;
    public Slider procrastinationResistanceSlider;

    // Reference to the Player script to access PlayerStats
    private Player player;

    void Start()
    {
        // Find the player in the scene and get the Player script component
        player = FindFirstObjectByType<Player>();
    }

    void Update()
    {
        // Update the sliders dynamically based on the player's current stats
        UpdateStatDisplay();
    }

    // Method to update all the stat sliders
    private void UpdateStatDisplay()
    {
        // Set slider values based on player stats
        energySlider.value = player.playerStats.energy;
        sanitySlider.value = player.playerStats.sanity;
        stressSlider.value = player.playerStats.stress;
        focusSlider.value = player.playerStats.focus;
    }

    // Optional: You can add methods to update individual sliders if needed
    public void UpdateStatSlider(string statName)
    {
        switch (statName)
        {
            case "Energy":
                energySlider.value = player.playerStats.energy;
                break;
            case "Sanity":
                sanitySlider.value = player.playerStats.sanity;
                break;
            case "Stress":
                stressSlider.value = player.playerStats.stress;
                break;
            case "Focus":
                focusSlider.value = player.playerStats.focus;
                break;
            case "Knowledge":
                knowledgeSlider.value = player.playerStats.knowledge;
                break;
            case "SocialLife":
                socialLifeSlider.value = player.playerStats.socialLife;
                break;
            case "Finances":
                financesSlider.value = player.playerStats.finances;
                break;
            case "Health":
                healthSlider.value = player.playerStats.health;
                break;
            case "Sleep":
                sleepSlider.value = player.playerStats.sleep;
                break;
            case "Motivation":
                motivationSlider.value = player.playerStats.motivation;
                break;
            case "Luck":
                luckSlider.value = player.playerStats.luck;
                break;
            case "ProcrastinationResistance":
                procrastinationResistanceSlider.value = player.playerStats.procrastinationResistance;
                break;
            default:
                Debug.LogWarning("Stat name not recognized.");
                break;
        }
    }
}