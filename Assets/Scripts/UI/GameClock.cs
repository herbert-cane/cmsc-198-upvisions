using UnityEngine;

public class FastClock : MonoBehaviour
{
    [Header("Speed")]
    public float timeScale = 60f; // 1 real second = 1 in-game minute

    [Header("Start Time")]
    [Range(0,23)] public int hour = 18;
    [Range(0,59)] public int minute = 0;

    float timer;

    void Update()
    {
        timer += Time.deltaTime * timeScale;

        if (timer >= 60f)
        {
            timer -= 60f;
            minute++;

            if (minute >= 60)
            {
                minute = 0;
                hour++;

                if (hour >= 24) hour = 0;
            }
        }
    }

    // 0 = midnight, 0.5 = noon, 1 = midnight
    public float GetNormalizedTimeOfDay()
    {
        return (hour + minute / 60f) / 24f;
    }
}
