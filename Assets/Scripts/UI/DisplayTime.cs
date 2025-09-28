using UnityEngine;
using TMPro;

public class DisplayTime : MonoBehaviour
{
    public TextMeshProUGUI timeText;  // Reference to the TextMeshPro component
    public FastClock clock;           // Reference to your FastClock script

    void Update()
    {
        if (clock == null || timeText == null) return;

        // Get in-game hour & minute from FastClock
        int hour = clock.hour;
        int minute = clock.minute;

        // Convert to 12-hour format with AM/PM
        string ampm = (hour >= 12) ? "PM" : "AM";
        int displayHour = hour % 12;
        if (displayHour == 0) displayHour = 12;

        // Format like 07:05 AM
        string formattedTime = string.Format("{0:00}:{1:00} {2}", displayHour, minute, ampm);

        // Show on UI
        timeText.text = formattedTime;
    }
}