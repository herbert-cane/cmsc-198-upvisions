using UnityEngine;
using UnityEngine.UI;

public class DynamicWeatherUI : MonoBehaviour
{
    public Image weatherImage; // UI Image component where the weather sprite will be displayed
    public Sprite[] weatherSprites; // Array to hold the sprites for weather and time of day (make sure to assign them in the Inspector)

    // For demonstration, we'll use these values to simulate the time of day and weather.
    // You can replace these with your in-game system parameters.
    public float currentTimeOfDay; // 0 = midnight, 1 = midday, 0.5 = sunrise, etc.
    public int currentWeatherIndex; // 0 = clear, 1 = cloudy, 2 = rainy, etc. (update this with your weather system)

    void Start()
    {
        // Make sure the weatherImage is assigned in the Inspector
        if (weatherImage == null)
        {
            Debug.LogError("WeatherImage is not assigned in the Inspector!");
            return;
        }

        // Set the weather image based on the time of day and current weather
        UpdateWeatherImage();
    }

    void Update()
    {
        // Continuously update the weather display (for simulation purposes)
        // In your game, replace this with actual time/weather system parameters
        UpdateWeatherImage();
    }

    // Update the weather image based on the current time and weather
    void UpdateWeatherImage()
    {
        // Use currentTimeOfDay to determine which row (time of day) to pick from the sprite sheet
        int timeOfDayRow = Mathf.FloorToInt(currentTimeOfDay * 4); // Assuming 4 rows in the sprite sheet (morning, noon, evening, night)

        // For simplicity, use currentWeatherIndex to pick which column (weather) to use
        int weatherColumn = currentWeatherIndex; // You should dynamically get this value based on weather in your scene

        // Calculate the correct sprite index from the sprite sheet grid
        int spriteIndex = timeOfDayRow * 4 + weatherColumn; // Assuming there are 4 columns for weather types

        if (spriteIndex >= 0 && spriteIndex < weatherSprites.Length)
        {
            weatherImage.sprite = weatherSprites[spriteIndex];
        }
        else
        {
            Debug.LogWarning("Invalid sprite index.");
        }
    }
}