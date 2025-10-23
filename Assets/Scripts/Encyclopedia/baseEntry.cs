using UnityEngine;

// This is an "abstract" class. You cannot create an "EncyclopediaEntry" asset directly.
// You must create one of its children (like PersonalityEntry, LocationEntry, etc.).
// This is the foundation that all entries share.

public abstract class EncyclopediaEntry : ScriptableObject
{
    [Header("Base Encyclopedia Info")]
    
    [Tooltip("Unique ID used for database lookups and <link> tags. e.g., 'prof_reyes', 'loc_cas_bldg'")]
    public string entryID;

    [Tooltip("The display name for the entry. e.g., 'Professor Reyes', 'CAS Building'")]
    public string entryName;

    [Tooltip("The icon shown in the list view.")]
    public Sprite icon;

    [Header("Knowledge State")]
    [Tooltip("The text to display when the entry is still locked. e.g., '???'")]
    [TextArea(2, 4)]
    public string lockedDescription = "???";

    [Tooltip("The first piece of info the player sees when they discover this entry.")]
    [TextArea(5, 10)]
    public string initialDescription;
    
    // You can add other shared properties here, like:
    // public bool startsUnlocked = false;
}