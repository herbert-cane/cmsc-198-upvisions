using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

public class MapTransition : MonoBehaviour
{
    [SerializeField] PolygonCollider2D transitionArea; // The new area's transition boundaries
    [SerializeField] Transform teleportLocation; // Teleport location for the player (destination)
    private CinemachineConfiner2D confiner;
    [SerializeField] Direction direction;
    [SerializeField] float transitionSpeed = 2f; // Transition speed

    public enum Direction { Up, Down, Left, Right }

    private void Awake()
    {
        // Find CinemachineConfiner2D component in the scene
        confiner = FindFirstObjectByType<CinemachineConfiner2D>();
        if (confiner == null)
        {
            Debug.LogError("CinemachineConfiner2D not found in the scene.");
        }
        else
        {
            Debug.Log("CinemachineConfiner2D found.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player or a child object of the player entered the waypoint's trigger area (BoxCollider2D)
        if (other.gameObject.CompareTag("Player") || other.gameObject.transform.parent.CompareTag("Player"))
        {
            confiner.BoundingShape2D = transitionArea;

            // Get the parent GameObject if the collider is part of the player’s child
            GameObject player = other.gameObject.CompareTag("Player") ? other.gameObject : other.gameObject.transform.parent.gameObject;
            
            UpdatePlayerPosition(player);
        }
        else
        {
            return;
        }
    }

    private void UpdatePlayerPosition(GameObject player)
    {
        // Teleport the player to the teleportLocation's position
        player.transform.position = teleportLocation.position;
    }
}