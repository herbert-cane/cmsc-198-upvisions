using UnityEngine;

using Unity.Cinemachine;

public class ZoneTransition : MonoBehaviour
{
    public CinemachineCamera virtualCamera;   // Reference to the Cinemachine camera
    public PolygonCollider2D exteriorCollider;       // Collider for the exterior zone
    public PolygonCollider2D interiorCollider;       // Collider for the interior zone
    public CinemachineConfiner2D confiner;             // Cinemachine Confiner2D component

    void Start()
    {
        // Initially set the confiner to the exterior zone
        confiner.BoundingShape2D = exteriorCollider;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // If the player enters the interior zone, update the confiner
            if (this.CompareTag("Interior"))
            {
                confiner.BoundingShape2D = interiorCollider; // Set confiner to interior zone
            }
            else if (this.CompareTag("Exterior"))
            {
                confiner.BoundingShape2D = exteriorCollider; // Set confiner to exterior zone
            }
        }
    }
}
