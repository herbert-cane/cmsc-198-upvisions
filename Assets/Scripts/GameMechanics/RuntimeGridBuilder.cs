using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class RuntimeGridBuilder : MonoBehaviour
{
    public bool buildOnStart = true;
    
    void Start()
    {
        if (buildOnStart)
        {
            StartCoroutine(BuildGridDelayed());
        }
    }
    
    IEnumerator BuildGridDelayed()
    {
        // Wait one frame to ensure everything is loaded
        yield return null;
        BuildScheduleGrid();
    }
    
    public void BuildScheduleGrid()
    {
        GameObject scheduleGridPanel = GameObject.Find("ScheduleGridPanel");
        if (scheduleGridPanel == null)
        {
            Debug.LogError("ScheduleGridPanel not found!");
            return;
        }
        
        // Use the same building methods from the editor script above
        // (Copy the BuildTimeLabelsColumn, BuildDayColumns methods here)
        
        Debug.Log("Schedule grid built at runtime!");
    }
    
    // Copy the helper methods from the editor script here too
    // CreateUIObject, AddLayoutGroup, AddLayoutElement, etc.
}