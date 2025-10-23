using UnityEngine;

[CreateAssetMenu(fileName = "New Personality", menuName = "Encyclopedia/Personality Entry")]
public class PersonalityEntry : EncyclopediaEntry
{
    [Header("Personality Details")]
    [Tooltip("The person's role. e.g., 'Professor, History Dept.', 'Registrar Staff', 'Classmate'")]
    public string role;

    [Tooltip("Info revealed at Knowledge Level 1. e.g., 'Office hours, teaching style.'")]
    [TextArea(5, 10)]
    public string knowledgeTier1;

    [Tooltip("Info revealed at Knowledge Level 2. e.g., 'Likes coffee, exam tips.'")]
    [TextArea(5, 10)]
    public string knowledgeTier2;

    [Tooltip("Info revealed at Knowledge Level 3. e.g., 'Personal details, secret lore.'")]
    [TextArea(5, 10)]
    public string knowledgeTier3;

    [Header("Cross-Links")]
    [Tooltip("Where does this person usually hang out?")]
    public LocationEntry defaultLocation;
}