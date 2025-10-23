using UnityEngine;
[RequireComponent(typeof(ParticleSystem))]
public class RainFIx : MonoBehaviour

    {
    [SerializeField] private Transform player;   // Assign your Player in Inspector
    public Vector3 offset = new Vector3(0, 5, 0); // Position above player

    void LateUpdate()
    {
        if (player != null)
        {
            // Force follow position only
            transform.position = player.position + offset;

            // Force rotation to stay upright (so it never tilts)
            transform.rotation = Quaternion.identity;

            // Just in case scale gets weird
            transform.localScale = Vector3.one;
        }
    }
}