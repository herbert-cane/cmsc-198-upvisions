using UnityEngine;

public class PauseController : MonoBehaviour
{
    public static bool isGamePaused { get; private set; } = false; // Static property to track if the game is paused
    public static void setPause(bool pause)
    {
        isGamePaused = pause;
    }
}
