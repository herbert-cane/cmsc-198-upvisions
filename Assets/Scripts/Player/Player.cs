using UnityEngine;

// PlayerStats remains a simple data class
[System.Serializable]
public class PlayerStats
{
    // Core Stats
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
}

// Player class that is MonoBehaviour and attached to the Player GameObject
public class Player : MonoBehaviour
{
    // PlayerStats will hold all the player’s data
    public PlayerStats playerStats;

    private void Start()
    {
        // Initialize stats with default values or load saved data if available
        playerStats = new PlayerStats()
        {
            energy = 100f,
            sanity = 100f,
            stress = 0f,
            focus = 100f,
            knowledge = 0f,
            socialLife = 50f,
            finances = 50f,
            health = 100f,
            sleep = 100f,
            motivation = 100f,
            luck = 50f,
            procrastinationResistance = 50f
        };
    }

    // You can add other methods for handling player actions like reducing energy, stress, etc.
}