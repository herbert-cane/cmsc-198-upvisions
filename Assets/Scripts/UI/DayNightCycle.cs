using UnityEngine;
using UnityEngine.Rendering.Universal; // for Light2D

public class DayNightCycle2D : MonoBehaviour
{
    [Header("References")]
    public FastClock clock;        // Reference to your FastClock script
    public Light2D sunLight;       // Global Light 2D
    public Transform sunSprite;    // Optional: sprite that moves across sky

    [Header("Lighting")]
    public Gradient lightColor;           // Colors over time
    public AnimationCurve lightIntensity; // Intensity over time

    [Header("Movement")]
    public float moveDistance = 20f;      // Left ↔ Right distance

    void Update()
    {
        if (clock == null) return;

        // Get time from FastClock (0 = midnight, 0.5 = noon, 1 = midnight)
        float t = clock.GetNormalizedTimeOfDay();

        // Update sun light
        if (sunLight != null)
        {
            sunLight.color = lightColor.Evaluate(t);
            sunLight.intensity = lightIntensity.Evaluate(t);
        }

        // Move sun sprite side-to-side
        if (sunSprite != null)
        {
            float x = Mathf.Lerp(-moveDistance, moveDistance, t);
            sunSprite.localPosition = new Vector3(x, sunSprite.localPosition.y, 0);
        }
    }
}
