using UnityEngine;

public class MapSpawner : MonoBehaviour
{
    public string spawnerID;

    [Header("Camera Hardcoded Boundaries")]
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    // Visual aid in the editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }

    private void OnDrawGizmosSelected()
{
    Gizmos.color = Color.red;
    // Calculate center and size based on your min/max numbers
    Vector3 center = new Vector3((minX + maxX) / 2, (minY + maxY) / 2, 0);
    Vector3 size = new Vector3(maxX - minX, maxY - minY, 1);
    Gizmos.DrawWireCube(center, size);
}
}