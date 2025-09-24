using UnityEngine;
using TMPro;

public class DisplayTime : MonoBehaviour
{
    public TextMeshProUGUI timeText;  // Reference to the TextMeshPro component to display the time

    void Update()
    {
        // Get the current system time
        System.DateTime currentTime = System.DateTime.Now;

        // Format the time in 12-hour format with AM/PM
        string formattedTime = currentTime.ToString("hh:mm:ss tt");

        // Set the formatted time to the TextMeshPro text component
        if (timeText != null)
        {
            timeText.text = formattedTime;
        }
    }
}
