using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUI : MonoBehaviour
{
    // References to the UI Image bars for stats display (empty bar images with filled type)
    public Image energyBar;
    public Image sanityBar;
    public Image stressBar;
    public Image focusBar;
    public Image knowledgeBar;
    public Image socialLifeBar;
    public Image financesBar;
    public Image healthBar;
    public Image sleepBar;
    public Image motivationBar;
    public Image luckBar;
    public Image procrastinationResistanceBar;

    // Reference to the Player script to access PlayerStats
    private Player player;

    void Start()
    {
        // Find the player in the scene and get the Player script component
        player = FindFirstObjectByType<Player>();
    }

    void Update()
    {
        // Update the image bars dynamically based on the player's current stats
        UpdateStatDisplay();
    }

    // Method to update all the stat bars (image fill)
    private void UpdateStatDisplay()
    {
        // Set fillAmount based on player stats (normalized between 0 and 1)
        energyBar.fillAmount = player.playerStats.energy / 100f;
        sanityBar.fillAmount = player.playerStats.sanity / 100f;
        stressBar.fillAmount = player.playerStats.stress / 100f;
        // focusBar.fillAmount = player.playerStats.focus / 100f;
        // knowledgeBar.fillAmount = player.playerStats.knowledge / 100f;
        // socialLifeBar.fillAmount = player.playerStats.socialLife / 100f;
        // financesBar.fillAmount = player.playerStats.finances / 100f;
        // healthBar.fillAmount = player.playerStats.health / 100f;
        // sleepBar.fillAmount = player.playerStats.sleep / 100f;
        // motivationBar.fillAmount = player.playerStats.motivation / 100f;
        // luckBar.fillAmount = player.playerStats.luck / 100f;
        // procrastinationResistanceBar.fillAmount = player.playerStats.procrastinationResistance / 100f;
    }

    // Optional: You can add methods to update individual bars if needed
    public void UpdateStatBar(string statName)
    {
        switch (statName)
        {
            case "Energy":
                energyBar.fillAmount = player.playerStats.energy / 100f;
                break;
            case "Sanity":
                sanityBar.fillAmount = player.playerStats.sanity / 100f;
                break;
            case "Stress":
                stressBar.fillAmount = player.playerStats.stress / 100f;
                break;
            // case "Focus":
            //     focusBar.fillAmount = player.playerStats.focus / 100f;
            //     break;
            // case "Knowledge":
            //     knowledgeBar.fillAmount = player.playerStats.knowledge / 100f;
            //     break;
            // case "SocialLife":
            //     socialLifeBar.fillAmount = player.playerStats.socialLife / 100f;
            //     break;
            // case "Finances":
            //     financesBar.fillAmount = player.playerStats.finances / 100f;
            //     break;
            // case "Health":
            //     healthBar.fillAmount = player.playerStats.health / 100f;
            //     break;
            // case "Sleep":
            //     sleepBar.fillAmount = player.playerStats.sleep / 100f;
            //     break;
            // case "Motivation":
            //     motivationBar.fillAmount = player.playerStats.motivation / 100f;
            //     break;
            // case "Luck":
            //     luckBar.fillAmount = player.playerStats.luck / 100f;
            //     break;
            case "ProcrastinationResistance":
                procrastinationResistanceBar.fillAmount = player.playerStats.procrastinationResistance / 100f;
                break;
            default:
                Debug.LogWarning("Stat name not recognized.");
                break;
        }
    }
}