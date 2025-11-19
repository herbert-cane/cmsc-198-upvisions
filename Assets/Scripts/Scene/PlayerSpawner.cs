using UnityEngine;
using TMPro; // If you want to show the name above their head

public class GameInitializer : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject malePrefab;
    public GameObject femalePrefab;

    [Header("Spawn Settings")]
    public Transform spawnPoint;

    void Start()
    {
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        // 1. Get the data
        string avatarID = SceneData.selectedAvatarID;
        string name = SceneData.playerName;
        string org = SceneData.selectedCourseID;

        GameObject prefabToSpawn = null;

        // 2. Pick the right prefab
        if (avatarID == "Boy")
        {
            prefabToSpawn = malePrefab;
        }
        else if (avatarID == "Girl")
        {
            prefabToSpawn = femalePrefab;
        }
        else 
        {
            // Fallback if something broke
            prefabToSpawn = malePrefab; 
        }

        // 3. Instantiate it
        GameObject playerInstance = Instantiate(prefabToSpawn, spawnPoint.position, Quaternion.identity);

        // 4. Apply the Name (Assuming your player has a script named 'PlayerStats')
        // You might need to find a TextMeshPro component on the player to change the name tag
        Player playerScript = playerInstance.GetComponent<Player>(); 
        if (playerScript != null)
        {
            playerScript.playerStats.playerName = name;
            playerScript.playerStats.org = org;
            // Optional: Update the academicProgram too if you want the full name
            // playerScript.playerStats.academicProgram = "Full Name Here"; 
        }

        // OPTIONAL: Find the name tag above the player's head and change it
        TextMeshPro nameTag = playerInstance.GetComponentInChildren<TextMeshPro>();
        if (nameTag != null)
        {
            nameTag.text = name;
        }
        
        Debug.Log($"Spawned {name} the {org} student!");
    }
}