using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public Player player; // Reference to the player script

    private string saveFilePath;

    private void Start()
    {
        saveFilePath = Application.persistentDataPath + "/playerStats.json";
        LoadStats();
    }

    // Save stats to JSON
    public void SaveStats()
    {
        string json = JsonUtility.ToJson(player.playerStats);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Game Saved!");
    }

    // Load stats from JSON
    public void LoadStats()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            player.playerStats = JsonUtility.FromJson<PlayerStats>(json);
            Debug.Log("Game Loaded!");
        }
        else
        {
            Debug.Log("No saved game found.");
        }
    }
}
