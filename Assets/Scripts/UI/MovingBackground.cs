using UnityEngine;

public class MovingBackground : MonoBehaviour
{
    [Tooltip("Units per second the background moves to the left")]
    [SerializeField] private float speed = 2f;

    [Tooltip("X position at or below which the background will be destroyed")]
    [SerializeField] private float destroyX = -20f;

    // Update is called once per frame
    void Update()
    {
        // Move left
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        // Remove when off-screen / past destroy limit
        if (transform.position.x <= destroyX)
        {
            Destroy(gameObject);
        }
    }
}
