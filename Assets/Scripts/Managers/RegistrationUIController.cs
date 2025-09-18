using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems; // Use TextMeshPro for better text rendering

public class RegistrationUIController : MonoBehaviour
{
    public static RegistrationUIController Instance;
    public GameObject registrationCanvas;
    public GameObject courseButtonPrefab;
    public GameObject timeSlotPrefab;

    public Transform courseListParent;
    public Transform scheduleGridParent;
    public TMP_Text selectedCoursesText;
    public TMP_Text conflictText;
    public TMP_Text totalUnitsText;
    public Button confirmButton;

    private ClassSchedulerPuzzleGenerator scheduler;
    private List<CourseOffering> currentOfferings;
    private Dictionary<string, ClassSchedule> selectedSchedules = new Dictionary<string, ClassSchedule>();
    private Dictionary<ScheduleType, bool> activeFilters = new Dictionary<ScheduleType, bool>();
    private int totalUnits = 0;

    private const int dayCount = 5; // Mon-Fri
    private const int timeSlotsPerDay = 12; // 7:30 AM to 6:00 PM in 1-hour intervals
    private GameObject[,] scheduleGridSlots; // 2D array to hold time slot references

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Initialize all filters as active
        foreach (ScheduleType type in System.Enum.GetValues(typeof(ScheduleType)))
        {
            activeFilters[type] = true;
        }
    }
    void Start()
    {
        // Initialize the grid
        InitializeScheduleGrid();
        // Hide UI on start
        registrationCanvas.SetActive(false);
    }

    public void UpdateFilter(ScheduleType filterType, bool isActive)
    {
        activeFilters[filterType] = isActive;
        RefreshCourseListDisplay();
    }

    private void RefreshCourseListDisplay()
    {
        // Clear current list
        foreach (Transform child in courseListParent)
        {
            Destroy(child.gameObject);
        }

        // Repopulate based on active filters
        PopulateCourseList();
    }
    public void OpenRegistrationTerminal()
    {
        registrationCanvas.SetActive(true);
        // Generate a new puzzle for the current semester
        scheduler = new ClassSchedulerPuzzleGenerator();
        currentOfferings = scheduler.GeneratePuzzleForSemester("Second Year", "First Semester");
        PopulateCourseList();
        ClearSelections();
    }

    private void InitializeScheduleGrid()
    {
        scheduleGridSlots = new GameObject[dayCount, timeSlotsPerDay];
        // Create grid headers (Days)
        for (int day = 0; day < dayCount; day++)
        {
            GameObject header = new GameObject($"Header-{(DayOfWeek)day}");
            header.AddComponent<TextMeshProUGUI>().text = ((DayOfWeek)day).ToString();
            header.transform.SetParent(scheduleGridParent);
        }

        // Create time slots
        for (int time = 0; time < timeSlotsPerDay; time++)
        {
            int hour = 7 + (time / 2); // Start at 7 AM
            int minute = (time % 2) * 30; // 00 or 30
            string timeLabel = $"{hour:00}:{minute:00}";

            // Create time label
            GameObject timeLabelObj = new GameObject($"TimeLabel-{timeLabel}");
            timeLabelObj.AddComponent<TextMeshProUGUI>().text = timeLabel;
            timeLabelObj.transform.SetParent(scheduleGridParent);

            for (int day = 0; day < dayCount; day++)
            {
                GameObject slot = Instantiate(timeSlotPrefab, scheduleGridParent);
                slot.name = $"Slot-{(DayOfWeek)day}-{timeLabel}";
                scheduleGridSlots[day, time] = slot;

                // Add a trigger to show tooltip or details on hover
                EventTrigger trigger = slot.AddComponent<EventTrigger>();
                // ... (Setup hover events to show class details)
            }
        }
    }

    private void PopulateCourseList()
    {
        foreach (CourseOffering offering in currentOfferings)
        {
            // Check if this course type should be shown based on filters
            bool shouldShow = ShouldShowCourse(offering);
            
            GameObject buttonObj = Instantiate(courseButtonPrefab, courseListParent);
            buttonObj.SetActive(shouldShow); // Hide if filtered out

            TMP_Text buttonText = buttonObj.GetComponentInChildren<TMP_Text>();
            buttonText.text = $"{offering.CourseNo}\n{offering.CourseTitle} ({offering.Units}u)";

            Button button = buttonObj.GetComponent<Button>();
            button.onClick.AddListener(() => OnCourseSelected(offering));

            // Gray out if filtered
            if (!shouldShow)
            {
                button.interactable = false;
                buttonText.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            }
        }
    }
    
        private bool ShouldShowCourse(CourseOffering course)
    {
        // For courses with both lecture and lab, show if either filter is active
        if (course.HasLaboratory)
        {
            return activeFilters[ScheduleType.Lecture] || activeFilters[ScheduleType.Laboratory];
        }
        
        // For lecture-only courses, check lecture filter
        return activeFilters[ScheduleType.Lecture];
    }

    private bool DoesScheduleConflict(ClassSchedule newSchedule, ICollection<ClassSchedule> existingSchedules)
    {
        foreach (ClassSchedule existingSchedule in existingSchedules)
        {
            if (newSchedule.ConflictsWith(existingSchedule))
            {
                return true;
            }
        }
        return false;
    }

    public void OnCourseSelected(CourseOffering course)
        {
            ClassSchedule chosenSection = null;
            foreach (ClassSchedule section in course.AvailableSchedules) // FIXED: Changed ClassSection to ClassSchedule
            {
                if (!DoesScheduleConflict(section, selectedSchedules.Values)) // Now this method exists
                {
                    chosenSection = section;
                    break;
                }
            }

            if (chosenSection != null)
            {
                selectedSchedules[course.CourseNo] = chosenSection;
                totalUnits += course.Units;
                UpdateVisualSchedule();
                UpdateStatusText();
            }
            else
            {
                Debug.LogWarning($"No non-conflicting section found for {course.CourseNo}!");
            }
        }

    private void UpdateVisualSchedule()
    {
        ClearScheduleGridHighlights();

        foreach (var schedule in selectedSchedules.Values)
        {
            Color scheduleColor = schedule.Type == ScheduleType.Lecture ? Color.blue : Color.green;
            Vector2Int[] gridPositions = ConvertScheduleToGridPositions(schedule);

            foreach (Vector2Int pos in gridPositions)
            {
                if (pos.x < dayCount && pos.y < timeSlotsPerDay)
                {
                    Image slotImage = scheduleGridSlots[pos.x, pos.y].GetComponent<Image>();
                    slotImage.color = scheduleColor;

                    // NOW schedule.CourseNo exists
                    TMP_Text slotText = scheduleGridSlots[pos.x, pos.y].GetComponentInChildren<TMP_Text>();
                    slotText.text = schedule.CourseNo;
                }
            }
        }
    }

    private Vector2Int[] ConvertScheduleToGridPositions(ClassSchedule schedule)
    {
        List<Vector2Int> positions = new List<Vector2Int>();
        int startMinuteTotal = schedule.StartHour * 60 + schedule.StartMinute;
        int startSlot = (startMinuteTotal - 450) / 30; // 7:30 AM = 450 minutes, each slot is 30 mins

        int durationSlots = schedule.DurationMinutes / 30;

        // For split schedules, we need to add positions for both days
        if (schedule.IsSplit)
        {
            AddSlotsForDay((int)schedule.Day1, startSlot, durationSlots, positions);
            AddSlotsForDay((int)schedule.Day2, startSlot, durationSlots, positions);
        }
        else
        {
            AddSlotsForDay((int)schedule.Day1, startSlot, durationSlots, positions);
        }
        return positions.ToArray();
    }

    private void AddSlotsForDay(int day, int startSlot, int durationSlots, List<Vector2Int> positions)
    {
        for (int i = 0; i < durationSlots; i++)
        {
            positions.Add(new Vector2Int(day, startSlot + i));
        }
    }

    private void ClearScheduleGridHighlights()
    {
        for (int day = 0; day < dayCount; day++)
        {
            for (int time = 0; time < timeSlotsPerDay; time++)
            {
                scheduleGridSlots[day, time].GetComponent<Image>().color = Color.white;
                scheduleGridSlots[day, time].GetComponentInChildren<TMP_Text>().text = "";
            }
        }
    }

    private void UpdateStatusText()
    {
        selectedCoursesText.text = "Selected Courses:\n";
        foreach (var entry in selectedSchedules)
        {
            selectedCoursesText.text += $"{entry.Key} - {entry.Value.ToString()}\n";
        }

        totalUnitsText.text = $"Total Units: {totalUnits}";
        if (totalUnits < 15) totalUnitsText.color = Color.yellow;
        else if (totalUnits > 18) totalUnitsText.color = Color.red;
        else totalUnitsText.color = Color.green;

        // Check for conflicts (should be none if we auto-picked, but good for UI)
        conflictText.text = CheckForConflicts() ? "❌ Schedule Conflict!" : "✅ No Conflicts";
    }

    private bool CheckForConflicts()
    {
        // Implementation to check if any selected schedules conflict
        List<ClassSchedule> schedules = new List<ClassSchedule>(selectedSchedules.Values);
        for (int i = 0; i < schedules.Count; i++)
        {
            for (int j = i + 1; j < schedules.Count; j++)
            {
                if (schedules[i].ConflictsWith(schedules[j])) return true;
            }
        }
        return false;
    }

    public void OnConfirmSchedule()
    {
        if (totalUnits < 15 || totalUnits > 18)
        {
            Debug.Log("Cannot confirm. Unit load must be between 15 and 18.");
            return;
        }
        if (CheckForConflicts())
        {
            Debug.Log("Cannot confirm. There are schedule conflicts.");
            return;
        }
        Debug.Log("Schedule confirmed!");
        // Save the schedule to the player's data
        registrationCanvas.SetActive(false);
        // Return to the game world
    }

    private void ClearSelections()
    {
        selectedSchedules.Clear();
        totalUnits = 0;
        ClearScheduleGridHighlights();
        UpdateStatusText();
    }
}