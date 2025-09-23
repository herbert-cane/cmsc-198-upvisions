using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public class Chest: MonoBehaviour, IInteractable
// {
//     public List<Item> items; // Items contained in the chest
//     private bool isOpened = false; // Flag to track if the chest has been opened

//     // Method to handle interaction with the chest
//     public void Interact()
//     {
//         if (!isOpened)
//         {
//             OpenChest();
//         }
//         else
//         {
//             Debug.Log("The chest is already opened.");
//         }
//     }

//     // Method to check if the chest can be interacted with
//     public bool CanInteract()
//     {
//         return !isOpened;
//     }

//     // Method to open the chest and give items to the player
//     private void OpenChest()
//     {
//         isOpened = true;
//         foreach (var item in items)
//         {
//             InventoryManager.Instance.AddItem(item); // Assuming an InventoryManager exists
//             Debug.Log("Added " + item.itemName + " to inventory.");
//         }
//         Debug.Log("Chest opened!");
//         // Optionally, play an animation or sound effect here
//     }
// }