using UnityEngine;
using System.Collections.Generic;

public class ToggleVisibility : MonoBehaviour
{
    // List to hold references to the GameObjects you want to toggle
    public List<GameObject> objectsToToggle;

    void Update()
    {
        // Check if the period (.) key is pressed
        if (Input.GetKeyDown(KeyCode.Period))
        {
            ToggleObjects();
        }
    }

    // Method to toggle the visibility of the GameObjects
    void ToggleObjects()
    {
        foreach (GameObject obj in objectsToToggle)
        {
            // Toggle the active state of each object in the list
            if (obj != null)
            {
                obj.SetActive(!obj.activeSelf);
            }
        }
    }
}
