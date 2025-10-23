using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform recTransform;
    private CanvasGroup canvasGroup;
    private Canvas parentCanvas; // Reference to the parent Canvas
    private Canvas currentCanvas; // Reference to the canvas during drag

    private void Awake()
    {
        recTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        // Automatically find the parent canvas
        parentCanvas = GetComponentInParent<Canvas>(); 
        if (parentCanvas == null)
        {
            Debug.LogError("Canvas not found in parent hierarchy!");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("OnBeginDrag");

        // Make the object semi-transparent while dragging
        canvasGroup.alpha = 0.6f; 
        canvasGroup.blocksRaycasts = false; // Disable raycasting (clicking) during drag

        // Set the current canvas for the dragged object
        currentCanvas = parentCanvas;
        if (currentCanvas != null)
        {
            // Reparent the dragged object to the current canvas to avoid disappearing when dragged out
            transform.SetParent(currentCanvas.transform);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("OnDrag");

        // Update the position of the dragged object based on the mouse movement in world space
        Vector2 movePosition = eventData.position / currentCanvas.scaleFactor; // Adjusting for canvas scale
        recTransform.position = movePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("OnEndDrag");

        // Restore the original opacity and re-enable raycasting (clicking) after the drag ends
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        // Check if the dragged object was dropped onto a new canvas
        if (eventData.pointerCurrentRaycast.gameObject != null)
        {
            Canvas dropCanvas = eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Canvas>();
            if (dropCanvas != null && dropCanvas != currentCanvas)
            {
                // Reparent the dragged object to the new canvas
                transform.SetParent(dropCanvas.transform);
                currentCanvas = dropCanvas; // Update the current canvas reference
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
       //Debug.Log("OnPointerDown");
    }
}
