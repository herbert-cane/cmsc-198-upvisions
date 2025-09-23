using UnityEngine;
using Unity.Cinemachine;

public class CameraZoom : MonoBehaviour
{
    public CinemachineCamera virtualCamera; // Reference to your Cinemachine Virtual Camera
    public float zoomSpeed = 2f;                   // Speed at which the zoom happens
    public float minSize = 5f;                     // Minimum orthographic size
    public float maxSize = 15f;                    // Maximum orthographic size
    private float targetSize;                      // The target orthographic size
    private float currentSize;                     // The current orthographic size

    public PolygonCollider2D mapCollider;         // Reference to the map's PolygonCollider2D for bounds

    private Vector3 cameraOffset;                  // The offset to prevent the camera from being outside bounds
    private float smoothZoomSpeed = 5f;            // Speed of the smooth zoom transition

    void Start()
    {
        if (virtualCamera == null)
        {
            virtualCamera = FindFirstObjectByType<CinemachineCamera>();
        }

        if (mapCollider == null)
        {
            Debug.LogError("MapCollider is not assigned!");
        }

        // Set the initial target size to the current camera size
        currentSize = virtualCamera.Lens.OrthographicSize;
        targetSize = currentSize;

        // Calculate the initial camera offset based on collider bounds
        SetCameraOffset();
    }

    void Update()
    {
        if (PauseController.isGamePaused) return; // Skip input processing if game is paused

        // Handle scroll wheel input
        HandleZoom();

        // Clamp the camera's position to ensure it stays within the bounds
        ClampCameraPosition();

        // Smoothly apply the zoom size
        currentSize = Mathf.Lerp(currentSize, targetSize, Time.deltaTime * smoothZoomSpeed);
        virtualCamera.Lens.OrthographicSize = currentSize;
    }

    void HandleZoom()
    {
        // Get the scroll wheel input (positive for zoom in, negative for zoom out)
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        // If the input is detected, adjust the target orthographic size
        if (scrollInput != 0f)
        {
            targetSize -= scrollInput * zoomSpeed;
            targetSize = Mathf.Clamp(targetSize, minSize, maxSize); // Clamp to the desired size range
        }
    }

    void SetCameraOffset()
    {
        // Get the bounds of the polygon collider
        Bounds mapBounds = mapCollider.bounds;

        // Calculate the camera offset based on the bounds
        // The offset ensures that the camera stays within the bounds when zooming
        cameraOffset = new Vector3(
            mapBounds.min.x + mapBounds.extents.x,
            mapBounds.min.y + mapBounds.extents.y,
            0f
        );
    }

    void ClampCameraPosition()
    {
        // Get the current camera position
        Vector3 cameraPosition = virtualCamera.transform.position;

        // Get the bounds of the PolygonCollider2D
        Bounds mapBounds = mapCollider.bounds;

        // Clamp the camera position to the map bounds
        cameraPosition.x = Mathf.Clamp(cameraPosition.x, mapBounds.min.x + cameraOffset.x, mapBounds.max.x - cameraOffset.x);
        cameraPosition.y = Mathf.Clamp(cameraPosition.y, mapBounds.min.y + cameraOffset.y, mapBounds.max.y - cameraOffset.y);

        // Apply the clamped position back to the camera
        virtualCamera.transform.position = cameraPosition;
    }
}