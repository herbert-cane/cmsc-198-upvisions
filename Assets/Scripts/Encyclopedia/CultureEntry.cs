using UnityEngine;

public enum CultureType { Jargon, History, Tradition, Myth }

[CreateAssetMenu(fileName = "New Culture Entry", menuName = "Encyclopedia/Culture Entry")]
public class CultureEntry : EncyclopediaEntry
{
    [Header("Culture Details")]
    public CultureType type;

    [Tooltip("The full definition or story. The 'initialDescription' can be just the rumor or the word itself.")]
    [TextArea(10, 20)]
    public string fullDescription;
}