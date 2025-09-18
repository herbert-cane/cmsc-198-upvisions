using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [Header("Assign your canvases")]
    public Canvas parallaxCanvas;
    public Canvas uiCanvas;

    void Start()
    {
        if (parallaxCanvas != null)
            parallaxCanvas.sortingOrder = 0;     // background

        if (uiCanvas != null)
            uiCanvas.sortingOrder = 10;          // foreground
    }
}
