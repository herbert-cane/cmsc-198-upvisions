using UnityEngine;
using Unity.Cinemachine; // Ensure you have the Cinemachine v3 package
using System.Linq;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance { get; private set; }

    private CinemachineCamera cinemachineCam;
    private CameraClamper clamper;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        
        // Find the new Cinemachine Camera component
        cinemachineCam = FindFirstObjectByType<CinemachineCamera>();
        if (cinemachineCam != null)
        {
            clamper = cinemachineCam.GetComponent<CameraClamper>();
        }
    }

    public void TeleportToMap(GameObject player, string targetID)
    {
        // 1. Find the target spawner
        MapSpawner target = FindObjectsByType<MapSpawner>(FindObjectsSortMode.None)
                            .FirstOrDefault(s => s.spawnerID == targetID);

        if (target != null)
        {
            // 2. Calculate Warp Delta (Distance traveled)
            Vector3 delta = target.transform.position - player.transform.position;

            // 3. Move Player
            player.transform.position = target.transform.position;

            // 4. Update the Hardcoded Boundaries
            if (clamper != null)
            {
                clamper.UpdateBoundaries(target.minX, target.maxX, target.minY, target.maxY);
            }

            // 5. Tell the Cinemachine Camera to Snap instantly
            if (cinemachineCam != null)
            {
                cinemachineCam.OnTargetObjectWarped(player.transform, delta);
            }
        }
    }
}