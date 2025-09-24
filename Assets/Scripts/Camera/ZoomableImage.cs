using UnityEngine;
using UnityEngine.UI;

public class ZoomableImage : MonoBehaviour
{
    [SerializeField] private Image imageToZoom;        // The Image UI element to zoom
    [SerializeField] private float zoomSpeed = 0.1f;    // Speed of zooming
    [SerializeField] private float minZoom = 0.5f;      // Minimum zoom scale
    [SerializeField] private float maxZoom = 2f;        // Maximum zoom scale

    private RectTransform rectTransform;   // Reference to the image's RectTransform

    void Start()
    {
        rectTransform = imageToZoom.GetComponent<RectTransform>();
    }

    void Update()
    {
        // Handle mouse scroll zoom
        HandleMouseZoom();

        // Handle pinch zoom (mobile support)
        HandleTouchPinchZoom();
    }

    private void HandleMouseZoom()
    {
        // Use mouse scroll wheel to zoom in/out
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0)
        {
            // Get current scale of the image
            float currentScale = rectTransform.localScale.x;

            // Adjust scale based on scroll input
            float newScale = currentScale + scrollInput * zoomSpeed;

            // Clamp the scale to be between minZoom and maxZoom
            newScale = Mathf.Clamp(newScale, minZoom, maxZoom);

            // Apply the new scale to the image
            rectTransform.localScale = new Vector3(newScale, newScale, 1);
        }
    }

    private void HandleTouchPinchZoom()
    {
        // For mobile devices, handle pinch-to-zoom with two fingers
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            // Calculate the distance between the two touches
            float currentDistance = Vector2.Distance(touch1.position, touch2.position);

            // Compare to previous distance to determine zoom direction
            if (touch1.phase == TouchPhase.Moved && touch2.phase == TouchPhase.Moved)
            {
                float previousDistance = Vector2.Distance(touch1.position - touch1.deltaPosition, touch2.position - touch2.deltaPosition);

                // If the distance is increasing, zoom in, if decreasing, zoom out
                float pinchAmount = currentDistance - previousDistance;

                // Adjust the image scale based on pinch amount
                float newScale = rectTransform.localScale.x + pinchAmount * zoomSpeed;

                // Clamp the scale between minZoom and maxZoom
                newScale = Mathf.Clamp(newScale, minZoom, maxZoom);

                // Apply the new scale to the image
                rectTransform.localScale = new Vector3(newScale, newScale, 1);
            }
        }
    }
}
