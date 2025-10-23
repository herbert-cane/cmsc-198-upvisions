using UnityEngine;

public enum ProcessType { Academic, Admin, StudentLife }

[CreateAssetMenu(fileName = "New Process", menuName = "Encyclopedia/Process Entry")]
public class ProcessEntry : EncyclopediaEntry
{
    [Header("Process Details")]
    public ProcessType type;
    
    [Tooltip("The steps are revealed one by one as the player learns them.")]
    public string[] steps;

    [Tooltip("A pro-tip unlocked after successfully completing the process once.")]
    [TextArea(3, 5)]
    public string masteryTip;

    [Header("Cross-Links")]
    [Tooltip("Where do you do this process?")]
    
    public LocationEntry relatedLocation;

    [Tooltip("Who do you need to talk to for this?")]
    public PersonalityEntry relatedPerson;
}