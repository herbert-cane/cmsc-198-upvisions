using UnityEngine;

[CreateAssetMenu(fileName = "NewQuest", menuName = "NPC/Quest")]
public class QuestSO : ScriptableObject
{
    public string questTitle;       // Title of the quest
    public string questDescription; // Description of the quest
    public bool isCompleted;        // If the quest is completed or not
}
