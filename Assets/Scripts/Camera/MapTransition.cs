using System.Collections;
using UnityEngine;
using Unity.Cinemachine; 

public class MapTransition : MonoBehaviour
{
    [Header("Transition Settings")]
    [SerializeField] PolygonCollider2D transitionArea; 
    [SerializeField] Transform teleportLocation; 
    
    // We need a reference to the actual Camera causing the issue
    private CinemachineCamera virtualCamera; 
    private CinemachineConfiner2D confiner;

    private void Awake()
    {
        confiner = FindFirstObjectByType<CinemachineConfiner2D>();
        
        // Find the active Cinemachine Camera
        virtualCamera = FindFirstObjectByType<CinemachineCamera>();

        if (confiner == null) Debug.LogError("CinemachineConfiner2D missing.");
        if (virtualCamera == null) Debug.LogError("CinemachineCamera missing.");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || (other.transform.parent != null && other.transform.parent.CompareTag("Player")))
        {
            GameObject player = other.CompareTag("Player") ? other.gameObject : other.transform.parent.gameObject;
            
            PerformTransition(player);
        }
    }

    private void PerformTransition(GameObject player)
    {
        // 1. Calculate the difference in movement
        Vector3 positionDelta = teleportLocation.position - player.transform.position;

        // 2. Teleport the Player
        player.transform.position = teleportLocation.position;

        // 3. Update the Confiner Shape
        if (confiner != null && transitionArea != null)
        {
            confiner.BoundingShape2D = transitionArea;
            
            // IMPORTANT: Tell the confiner the shape changed to prevent caching errors
            confiner.InvalidateBoundingShapeCache(); 
        }

        // 4. THE FIX: Warp the Camera
        // This tells Cinemachine: "Don't smooth pan, just snap to the new spot."
        if (virtualCamera != null)
        {
            virtualCamera.OnTargetObjectWarped(player.transform, positionDelta);
        }
    }
}