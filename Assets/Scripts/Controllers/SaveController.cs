using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.Cinemachine;

public class SaveController : MonoBehaviour
{
    private string saveLocation;
    public Player player;

    void Start()
    {
        string saveDirectory = Path.Combine(Application.dataPath, "Save");  // Custom directory inside the "Assets" folder
        if (!Directory.Exists(saveDirectory))
        {
            Directory.CreateDirectory(saveDirectory);  // Create directory if it doesn't exist
        }
        saveLocation = Path.Combine(saveDirectory, "savefile.json");  // Full path to save file
        
        LoadGame();
    }

    public void SaveGame()
    {
        if (player == null)
        {
            Debug.LogError("Player object not found.");
            return;
        }

        SaveData saveData = new SaveData
        {
            playerPosition = player.transform.position,
            mapBoundary = FindFirstObjectByType<CinemachineConfiner2D>().BoundingShape2D.gameObject.name,

            // Save all player stats
            energy = player.playerStats.energy,
            sanity = player.playerStats.sanity,
            stress = player.playerStats.stress,
            focus = player.playerStats.focus,
            knowledge = player.playerStats.knowledge,
            socialLife = player.playerStats.socialLife,
            finances = player.playerStats.finances,
            health = player.playerStats.health,
            sleep = player.playerStats.sleep,
            motivation = player.playerStats.motivation,
            luck = player.playerStats.luck,
            procrastinationResistance = player.playerStats.procrastinationResistance
        };

        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));
    }


    public void LoadGame()
    {

        if (player == null)
        {
            Debug.LogError("Player object not found in the scene.");
            return;  // Exit early if player is not found
        }

        if (File.Exists(saveLocation))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));

            // Load player position and stats
            player.transform.position = saveData.playerPosition;
            FindFirstObjectByType<CinemachineConfiner2D>().BoundingShape2D = GameObject.Find(saveData.mapBoundary).GetComponent<PolygonCollider2D>();

            player.playerStats.energy = saveData.energy;
            player.playerStats.sanity = saveData.sanity;
            player.playerStats.stress = saveData.stress;
            player.playerStats.focus = saveData.focus;
            player.playerStats.knowledge = saveData.knowledge;
            player.playerStats.socialLife = saveData.socialLife;
            player.playerStats.finances = saveData.finances;
            player.playerStats.health = saveData.health;
            player.playerStats.sleep = saveData.sleep;
            player.playerStats.motivation = saveData.motivation;
            player.playerStats.luck = saveData.luck;
            player.playerStats.procrastinationResistance = saveData.procrastinationResistance;
        }
        else
        {
            Debug.LogWarning("No saved game found. Creating a new save.");
            SaveGame(); // Save a new game if no save file exists
        }
    }

}