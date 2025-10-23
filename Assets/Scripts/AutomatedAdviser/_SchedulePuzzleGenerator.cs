using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SchedulePuzzleManager : MonoBehaviour
{
    [Header("References")]
    public AutomatedAdviser adviser;
    public Transform subjectListParent;      // Parent container for subject cards
    public GameObject subjectCardPrefab;     // Prefab for each subject card
    public Transform selectedScheduleParent; // Parent for confirmed selections

    [Header("UI Feedback")]
    public Color normalColor = Color.white;
    public Color conflictColor = new Color(1f, 0.5f, 0.5f);

    private List<ClassSchedule> availableSchedules = new();
    private List<ClassSchedule> selectedSchedules = new();
    private List<GameObject> spawnedCards = new();

    void Start()
    {
        if (adviser == null)
            adviser = FindAnyObjectByType<AutomatedAdviser>();

        // Subscribe to the event from AutomatedAdviser
        adviser.OnSchedulesGenerated += LoadAndDisplaySubjects;

        // Initially load subjects if schedules are already generated
        if (adviser.GetGeneratedSchedules().Count > 0)
        {
            LoadAndDisplaySubjects();
        }
    }

    // Pulls generated schedules from adviser and populates UI
void LoadAndDisplaySubjects()
{
    availableSchedules = adviser.GetGeneratedSchedules();

    if (availableSchedules == null || availableSchedules.Count == 0)
    {
        Debug.LogWarning("No available schedules to display.");
        return;
    }

    foreach (var sched in availableSchedules)
    {
        GameObject card = Instantiate(subjectCardPrefab, subjectListParent);
        spawnedCards.Add(card);

        // Access the text components within the prefab
        TMP_Text courseNameText = card.transform.Find("CourseNameText").GetComponent<TMP_Text>();
        TMP_Text sectionText = card.transform.Find("SectionText").GetComponent<TMP_Text>();
        TMP_Text dayText = card.transform.Find("DayText").GetComponent<TMP_Text>();
        TMP_Text timeSlotText = card.transform.Find("TimeSlotText").GetComponent<TMP_Text>();
        TMP_Text unitText = card.transform.Find("UnitText").GetComponent<TMP_Text>();

        // Update the text components with the data from the schedule
        if (courseNameText != null)
            courseNameText.text = sched.subjectCode; // or use sched.subjectTitle depending on what you need
        if (sectionText != null)
            sectionText.text = sched.section;
        if (dayText != null)
            dayText.text = sched.day;
        if (timeSlotText != null)
            timeSlotText.text = sched.timeSlot;
        if (unitText != null)
            unitText.text = $"{sched.units} units";

        Button button = card.GetComponent<Button>();
        if (button != null)
        {
            var tempSched = sched; // avoid closure capture
            button.onClick.AddListener(() => OnSubjectClicked(card, tempSched));
        }
    }
}

    void OnSubjectClicked(GameObject card, ClassSchedule selected)
    {
        if (selectedSchedules.Contains(selected))
        {
            selectedSchedules.Remove(selected);
            card.GetComponent<Image>().color = normalColor;
        }
        else
        {
            selectedSchedules.Add(selected);
            bool hasConflict = CheckForConflicts(selected);
            card.GetComponent<Image>().color = hasConflict ? conflictColor : normalColor;
        }
    }

    bool CheckForConflicts(ClassSchedule newSched)
    {
        foreach (var existing in selectedSchedules)
        {
            if (existing != newSched && existing.ConflictsWith(newSched))
            {
                Debug.LogWarning($"Conflict detected: {existing.subjectCode} and {newSched.subjectCode}");
                return true;
            }
        }
        return false;
    }

    public void ConfirmSelection()
    {
        foreach (Transform child in selectedScheduleParent)
            Destroy(child.gameObject);

        foreach (var sched in selectedSchedules)
        {
            GameObject card = Instantiate(subjectCardPrefab, selectedScheduleParent);
            Text label = card.GetComponentInChildren<Text>();
            if (label != null)
                label.text = $"{sched.subjectCode} - {sched.day} {sched.timeSlot} ({sched.section})";
        }

        Debug.Log($"Confirmed {selectedSchedules.Count} subjects for enrollment.");
    }
}