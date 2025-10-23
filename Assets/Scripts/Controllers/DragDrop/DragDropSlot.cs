using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDropSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");

        // Ensure the dragged object is not null and correctly places itself in the drop slot
        if (eventData.pointerDrag != null)
        {
            // Get the dragged object's RectTransform
            RectTransform draggedRect = eventData.pointerDrag.GetComponent<RectTransform>();

            // Position the dragged object in the center of the drop slot
            draggedRect.position = transform.position;  // Align with the slot's position
        }
    }
}
