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
        // Check if the player entered the waypoint's trigger area (BoxCollider2D)
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player entered the waypoint trigger.");

            // Start teleportation coroutine and update confiner
            StartCoroutine(TeleportPlayer(other.gameObject));
        }
        else
        {
            Debug.Log("Non-player object entered the waypoint.");
        }
    }

    private IEnumerator TeleportPlayer(GameObject player)
    {
        Debug.Log("Starting teleportation sequence.");
        
        // Log player position before teleportation
        Debug.Log("Player current position: " + player.transform.position);

        // Optional: Add fade-out or transition effect here if desired
        Debug.Log("Waiting for transition...");

        // Simulate transition delay
        yield return new WaitForSeconds(transitionSpeed);

        // Check if teleportLocation is set
        if (teleportLocation != null)
        {
            // Get world position of teleportLocation
            Vector3 worldPosition = teleportLocation.position; // Get world position of Transform
            Debug.Log("Teleporting player to: " + worldPosition);

            // Check if the player has a Rigidbody2D
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Use Rigidbody2D.MovePosition to move the player
                rb.MovePosition(worldPosition);
                Debug.Log("Player moved using Rigidbody2D.");
            }
            // After teleporting, update the Cinemachine confiner bounding shape
            if (confiner != null)
            {
                confiner.BoundingShape2D = transitionArea; // Update to the new area
                Debug.Log("Cinemachine confiner bounding shape updated.");
            }
        }
        else
        {
            Debug.LogError("Teleport location is not set!");
        }

        // Optionally add a fade-in effect after teleportation if desired
        Debug.Log("Teleportation completed.");
    }
}