using System;
using UnityEngine;

[Serializable]
public class ClassSchedule
{
    [Header("Subject Info")]
    public string subjectCode;
    public string subjectTitle;
    public int units;

    [Header("Schedule Info")]
    public string section;
    public string day;        // e.g., "Mon", "Tue", "Wed"
    public string timeSlot;   // e.g., "07:30-09:00"

    /// <summary>
    /// Determines if this schedule conflicts with another (same day and overlapping time).
    /// </summary>
    public bool ConflictsWith(ClassSchedule other)
    {
        if (day != other.day) return false;

        // Parse time ranges
        (int start1, int end1) = ParseTimeRange(timeSlot);
        (int start2, int end2) = ParseTimeRange(other.timeSlot);

        // Overlaps if times intersect
        bool overlap = (start1 < end2 && end1 > start2);
        return overlap;
    }

    /// <summary>
    /// Converts a "HH:MM-HH:MM" range into integer minutes for comparison.
    /// </summary>
    private (int start, int end) ParseTimeRange(string range)
    {
        try
        {
            string[] parts = range.Split('-');
            int start = ParseTimeToMinutes(parts[0]);
            int end = ParseTimeToMinutes(parts[1]);
            return (start, end);
        }
        catch
        {
            Debug.LogWarning($"Invalid time range format: {range}");
            return (0, 0);
        }
    }

    /// <summary>
    /// Converts "HH:MM" to total minutes.
    /// </summary>
    private int ParseTimeToMinutes(string time)
    {
        string[] parts = time.Split(':');
        int hour = int.Parse(parts[0]);
        int minute = int.Parse(parts[1]);
        return hour * 60 + minute;
    }

    /// <summary>
    /// Human-readable version for debugging and UI.
    /// </summary>
    public override string ToString()
    {
        return $"{subjectCode} ({section}) - {day} {timeSlot}";
    }
}
