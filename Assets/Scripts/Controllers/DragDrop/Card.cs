using UnityEngine;

public class Card : MonoBehaviour
{
    private BoxCollider2D col;
    private Vector3 startDragPosition;

    private void Start()
    {
        col = GetComponent<BoxCollider2D>();
    }

    private void OnMouseDown()
    {
        startDragPosition = transform.position;
        transform.position = GetMousePositionInWorldSpace();
    }

    private void OnMouseDrag()
    {
        transform.position = GetMousePositionInWorldSpace();
    }

    private void OnMouseUp()
    {
        col.enabled = false; // Temporarily disable collider to avoid self-collision
        BoxCollider2D hitCollider = (BoxCollider2D)Physics2D.OverlapPoint(transform.position);
        col.enabled = true; // Re-enable collider
        if (hitCollider != null && hitCollider.TryGetComponent(out ICardDropArea cardDropArea))
        {
            cardDropArea.OnCardDropped(this);
        }
        else
        {
            transform.position = startDragPosition; // Return to original position if not dropped in a valid area
        }
    }

    private Vector3 GetMousePositionInWorldSpace()
    {
        Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        p.z = 0; // Assuming a 2D game in the XY plane
        return p;
    }
}