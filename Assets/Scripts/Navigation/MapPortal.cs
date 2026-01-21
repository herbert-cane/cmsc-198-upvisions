using UnityEngine;

public class MapPortal : MonoBehaviour
{
    [SerializeField] private string targetSpawnerID;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object or its parent is the Player
        if (other.CompareTag("Player") || (other.transform.parent != null && other.transform.parent.CompareTag("Player")))
        {
            GameObject player = other.CompareTag("Player") ? other.gameObject : other.transform.parent.gameObject;
            
            // Call the correct method name in TransitionManager
            TransitionManager.Instance.TeleportToMap(player, targetSpawnerID);
        }
    }
}