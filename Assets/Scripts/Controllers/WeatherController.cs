using UnityEngine;

public class WeatherController : MonoBehaviour
{
    public enum WeatherType { Clear, Rainy }

    [Header("Current Weather")]
    public WeatherType currentWeather = WeatherType.Clear;

    [Header("Rain Effects")]
    public GameObject rainEffect; // Particle System prefab for rain

    [Header("References")]
    public FastClock clock;  // Link to FastClock

    int lastDay = -1; // Track when a new day starts

    void Update()
    {
        if (clock == null) return;

        // Check if a new in-game day has started
        int currentDay = clock.hour / 24; // crude way: 1 cycle per 24h
        if (clock.hour == 6 && clock.minute == 0 && currentDay != lastDay) // At 6:00 AM decide weather
        {
            RandomizeWeather();
            lastDay = currentDay;
        }

        // Toggle effects
        if (rainEffect != null)
            rainEffect.SetActive(currentWeather == WeatherType.Rainy);
    }

    void RandomizeWeather()
    {
        // 30% chance of rain
        int roll = Random.Range(0, 100);
        if (roll < 30)
            currentWeather = WeatherType.Rainy;
        else
            currentWeather = WeatherType.Clear;

        Debug.Log("Today's weather: " + currentWeather);
    }
}