using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TimeSlotTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string courseDetails = "";
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!string.IsNullOrEmpty(courseDetails))
        {
            // Show tooltip with course details
            TooltipManager.Instance.ShowTooltip(courseDetails);
        }
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.Instance.HideTooltip();
    }
    
    public void SetCourseDetails(string details)
    {
        courseDetails = details;
        GetComponentInChildren<TMP_Text>().text = details.Split('\n')[0]; // Show abbreviated text
    }
    
    public void ClearCourseDetails()
    {
        courseDetails = "";
        GetComponentInChildren<TMP_Text>().text = "";
    }
}