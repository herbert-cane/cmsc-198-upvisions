using UnityEngine;
using Unity.Cinemachine;

// This attribute allows us to see the extension in the Cinemachine Camera component
[SaveDuringPlay] 
[AddComponentMenu("")] // Hide from Add Component menu to keep it clean
public class CameraClamper : CinemachineExtension
{
    private float minX, maxX, minY, maxY;
    private bool isClampingActive = false;

    public void UpdateBoundaries(float xMin, float xMax, float yMin, float yMax)
    {
        minX = xMin;
        maxX = xMax;
        minY = yMin;
        maxY = yMax;
        isClampingActive = true;
    }

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        // We only care about the Body stage (after the camera has moved)
        if (stage == CinemachineCore.Stage.Body && isClampingActive)
        {
            Vector3 pos = state.RawPosition;

            // Get camera properties for "Edge Clamping"
            float orthoSize = state.Lens.OrthographicSize;
            float aspectRatio = state.Lens.Aspect;
            float halfWidth = orthoSize * aspectRatio;

            // Apply the Math.Clamp to the RawPosition
            pos.x = Mathf.Clamp(pos.x, minX + halfWidth, maxX - halfWidth);
            pos.y = Mathf.Clamp(pos.y, minY + orthoSize, maxY - orthoSize);

            // Send the corrected position back to Cinemachine
            state.RawPosition = pos;
        }
    }
}