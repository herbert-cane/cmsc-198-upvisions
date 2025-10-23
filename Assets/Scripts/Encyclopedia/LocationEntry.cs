using UnityEngine;

public enum LocationType { Academic, Service, Hangout, Landmark, OffCampus }

[CreateAssetMenu(fileName = "New Location", menuName = "Encyclopedia/Location Entry")]
public class LocationEntry : EncyclopediaEntry
{
    [Header("Location Details")]
    public LocationType type;

    [Tooltip("e.g., 'Mon-Fri, 8:00 AM - 5:00 PM', or '24/7'")]
    public string operatingHours;

    [Tooltip("The full, detailed description unlocked after exploring.")]
    [TextArea(10, 15)]
    public string fullDescription;

    [Header("Cross-Links")]
    [Tooltip("A list of 'entryID's for processes you can do here.")]
    public string[] relatedProcessIDs; // <-- CHANGED

    [Tooltip("A list of 'entryID's for people you can find here.")]
    public string[] notablePersonalityIDs; // <-- CHANGED
}