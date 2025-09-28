using UnityEngine;

public class DraggableCourse : MonoBehaviour
{
    private Vector3 offset;

    private void OnMouseDown()
    {
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseDrag()
    {
        Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(currentMousePosition.x + offset.x, currentMousePosition.y + offset.y, 0);
    }

    private void OnMouseUp()
    {
        SnapToGrid();
    }

    private void SnapToGrid()
    {
        // Here, you'll implement snapping logic based on the grid system you've set up
        // For simplicity, assume the grid is 1 unit per slot.
        Vector3 gridPosition = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), 0);
        transform.position = gridPosition;
    }
}
