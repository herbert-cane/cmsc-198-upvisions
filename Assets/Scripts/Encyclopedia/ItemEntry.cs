using UnityEngine;

public enum ItemType { Consumable, Academic, KeyItem }

[CreateAssetMenu(fileName = "New Item", menuName = "Encyclopedia/Item Entry")]
public class ItemEntry : EncyclopediaEntry
{
    [Header("Item Details")]
    public ItemType type;

    [Tooltip("What this item does. e.g., '+20 Energy', 'Required for History Midterm'")]
    public string effect;
    
    public int cost;

    [Header("Cross-Links")]
    [Tooltip("Where can you buy or find this item?")]
    public LocationEntry[] foundAtLocations;
}