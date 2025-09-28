using UnityEngine;
[System.Serializable]
public class PlayerStats
{
    // Core Stats
    public string yearLevel = "Second Year";
    public string semester = "First Semester";
    public float energy;
    public float sanity;
    public float stress;
    public float focus;

    // Secondary Stats
    public float knowledge;
    public float socialLife;
    public float finances;
    public float health;
    public float sleep;

    // Hidden Stats
    public float motivation;
    public float luck;
    public float procrastinationResistance;
    public SaveController saveController;
}

// Player class that is MonoBehaviour and attached to the Player GameObject
public class Player : MonoBehaviour
{
    // PlayerStats will hold all the player’s data
    public PlayerStats playerStats;
    public SaveController saveController;

    void Start()
    {
        // Check if saveController is assigned
        if (saveController == null)
        {
            saveController = FindFirstObjectByType<SaveController>(); // Find it dynamically if not assigned
        }

        // Load the player stats after ensuring saveController is assigned
        LoadPlayerStats();
        Debug.Log("Player stats loaded.");

    }

    // Method to load player stats from saved data or initialize with defaults
    private void LoadPlayerStats()
    {
        // Ensure saveController is set and load game data
        if (saveController != null)
        {
            saveController.LoadGame();
        }
        else
        {
            Debug.LogError("SaveController is not assigned.");
        }
    }
}