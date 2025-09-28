using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class RegistrationUIController : MonoBehaviour
{
    public Player player; // Reference to the Player class to get yearLevel and semester
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

    public int timeSlotsPerDay = 12; // 7:30 AM to 6:00 PM in 1-hour intervals

    private int dayCount = 6; // Mon-Sat

    private ClassSchedulerPuzzleGenerator scheduler;
    private List<CourseSubjectData> currentOfferings;
    private Dictionary<string, ClassSchedule> selectedSchedules = new Dictionary<string, ClassSchedule>();
    private Dictionary<ScheduleType, bool> activeFilters = new Dictionary<ScheduleType, bool>();
    private int totalUnits = 0;

    private GameObject[,] scheduleGridSlots; // 2D array to hold time slot references

    void Awake()
    {
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
        // PopulateCourseList(); // Moved to OpenRegistrationTerminal
        OpenRegistrationTerminal();
        UpdateStatusText();
    }

    public void UpdateFilter(ScheduleType filterType, bool isActive)
    {
        activeFilters[filterType] = isActive;
        RefreshCourseListDisplay();
    }

    private void RefreshCourseListDisplay()
    {
        foreach (Transform child in courseListParent)
        {
            Destroy(child.gameObject);
        }

        PopulateCourseList();
    }

    public void OpenRegistrationTerminal()
    {
        Debug.Log($"Canvas Active: {registrationCanvas.activeSelf}");
        registrationCanvas.SetActive(true);

        // Fetch year level and semester from PlayerStats
        string yearLevel = player.playerStats.yearLevel;
        string semester = player.playerStats.semester;

        Debug.Log($"Player Year Level: {yearLevel}, Semester: {semester}");

        // Use the player data (year level and semester) to generate the schedule
        scheduler = gameObject.AddComponent<ClassSchedulerPuzzleGenerator>();

        // Generate a new puzzle for the current semester
        currentOfferings = scheduler.GeneratePuzzleForSemester(yearLevel, semester);

        // Populate the course list
        PopulateCourseList();

        ClearSelections();
    }

    private void InitializeScheduleGrid()
    {
        // Ensure scheduleGridSlots is initialized
        if (scheduleGridSlots == null)
        {
            scheduleGridSlots = new GameObject[dayCount, timeSlotsPerDay];
        }

        // Ensure scheduleGridParent is assigned
        if (scheduleGridParent == null)
        {
            Debug.LogError("scheduleGridParent is not assigned in the Inspector!");
            return;
        }

        // Ensure timeSlotPrefab is assigned
        if (timeSlotPrefab == null)
        {
            Debug.LogError("timeSlotPrefab is not assigned in the Inspector!");
            return;
        }

        // Create grid headers (Days)
        for (int day = 0; day < dayCount; day++)
        {
            GameObject header = new GameObject($"Header-{(DayOfWeek)day}");
            header.AddComponent<TextMeshProUGUI>().text = ((DayOfWeek)day).ToString();
            header.transform.SetParent(scheduleGridParent);

            // Make sure the header elements are positioned correctly
            RectTransform headerRect = header.GetComponent<RectTransform>();
            headerRect.sizeDelta = new Vector2(100, 40); // Adjust size as needed
        }

        // Create time slots
        for (int time = 0; time < timeSlotsPerDay; time++)
        {
            int hour = 7 + (time / 2); // Start at 7 AM
            int minute = (time % 2) * 30; // 00 or 30
            string timeLabel = $"{hour:00}:{minute:00}";

            // Create time label (for 7:30 AM, 8:30 AM, etc.)
            GameObject timeLabelObj = new GameObject($"TimeLabel-{timeLabel}");
            timeLabelObj.AddComponent<TextMeshProUGUI>().text = timeLabel;
            timeLabelObj.transform.SetParent(scheduleGridParent);

            // Adjust RectTransform for time label positioning
            RectTransform timeLabelRect = timeLabelObj.GetComponent<RectTransform>();
            timeLabelRect.sizeDelta = new Vector2(100, 40); // Adjust size as needed

            // Create the time slot objects below the time labels
            for (int day = 0; day < dayCount; day++)
            {
                GameObject slot = Instantiate(timeSlotPrefab, scheduleGridParent);
                slot.name = $"Slot-{(DayOfWeek)day}-{timeLabel}";
                scheduleGridSlots[day, time] = slot;

                // Add trigger or functionality (e.g., tooltips, course details on hover)
                EventTrigger trigger = slot.AddComponent<EventTrigger>();
            }
        }
    }

    private void BuildDayHeaders()
    {
        string[] days = { "MON", "TUE", "WED", "THU", "FRI", "SAT" };

        // Create headers for each day (Mon-Sat) in the first row
        for (int day = 0; day < dayCount; day++)
        {
            string dayName = days[day];
            GameObject header = new GameObject($"Header-{dayName}");
            header.transform.SetParent(scheduleGridParent);

            // Add a TextMeshPro component for the header
            TMP_Text headerText = header.AddComponent<TextMeshProUGUI>();
            headerText.text = dayName;
            headerText.fontSize = 14;
            headerText.alignment = TextAlignmentOptions.Center;

            // Set RectTransform for positioning (optional)
            RectTransform headerRect = header.GetComponent<RectTransform>();
            headerRect.sizeDelta = new Vector2(100, 40); // Adjust size as needed
        }
    }

    private void BuildTimeLabelsColumn()
    {
        string[] timeLabels = { "7:30", "8:00", "8:30", "9:00", "9:30", "10:00", "10:30", "11:00", "11:30", "12:00", "12:30", "1:00" };

        // Create a container for time labels
        GameObject timeLabelContainer = new GameObject("TimeLabelContainer");
        timeLabelContainer.transform.SetParent(scheduleGridParent);

        // Create the time labels (7:30, 8:00, etc.)
        for (int time = 0; time < timeSlotsPerDay; time++)
        {
            string timeLabel = timeLabels[time];

            // Create a new game object for each time label
            GameObject timeLabelObj = new GameObject($"TimeLabel-{timeLabel}");
            timeLabelObj.transform.SetParent(timeLabelContainer.transform);

            // Add TextMeshPro component to display the time
            TMP_Text timeLabelText = timeLabelObj.AddComponent<TextMeshProUGUI>();
            timeLabelText.text = timeLabel;
            timeLabelText.fontSize = 12;
            timeLabelText.alignment = TextAlignmentOptions.Center;

            // Set RectTransform for positioning (optional)
            RectTransform timeLabelRect = timeLabelObj.GetComponent<RectTransform>();
            timeLabelRect.sizeDelta = new Vector2(100, 40); // Adjust size as needed
        }
    }

    private void BuildTimeSlotButtons()
    {
        for (int time = 0; time < timeSlotsPerDay; time++)
        {
            for (int day = 0; day < dayCount; day++)
            {
                GameObject slot = Instantiate(timeSlotPrefab, scheduleGridParent);
                slot.name = $"Slot-{(DayOfWeek)day}-{time}";

                // Store the slot in the grid array for later access
                scheduleGridSlots[day, time] = slot;

                // Set slot button's position (You can adjust layout as needed)
                RectTransform slotRect = slot.GetComponent<RectTransform>();
                slotRect.localPosition = new Vector3(0, 0, 0); // Center slot within grid container

                // Add functionality when a time slot is clicked
                Button button = slot.GetComponent<Button>();
                button.onClick.AddListener(() => OnTimeSlotClicked(day, time));
            }
        }
    }

    private void OnTimeSlotClicked(int day, int time)
    {
        // Handle the click on a time slot (e.g., display course info, add to selected schedule)
        Debug.Log($"Time slot clicked: Day-{(DayOfWeek)day} Time-{(time / 2) + 7}:{(time % 2) * 30}");
    }

    private void PopulateCourseList()
    {
        if (currentOfferings == null || currentOfferings.Count == 0)
        {
            Debug.LogWarning("No courses to display.");
            return;
        }

        foreach (CourseSubjectData course in currentOfferings)
        {
            bool shouldShow = ShouldShowCourse(course);

            GameObject buttonObj = Instantiate(courseButtonPrefab, courseListParent);
            buttonObj.SetActive(shouldShow); // Hide if filtered out

            TMP_Text buttonText = buttonObj.GetComponentInChildren<TMP_Text>();
            buttonText.text = $"{course.courseCode}\n{course.courseTitle} ({course.units}u)";

            Button button = buttonObj.GetComponent<Button>();
            button.onClick.AddListener(() => OnCourseSelected(course));

            // Gray out if filtered
            if (!shouldShow)
            {
                button.interactable = false;
                buttonText.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            }
        }
    }

    private bool ShouldShowCourse(CourseSubjectData course)
    {
        if (course.lablec.Contains("LB"))
        {
            return activeFilters[ScheduleType.Lecture] || activeFilters[ScheduleType.Laboratory];
        }

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

    public void OnCourseSelected(CourseSubjectData course)
    {
        ClassSchedule chosenSection = null;
        List<ClassSchedule> generatedSchedules = scheduler.GenerateSchedulesForCourse(course);

        foreach (ClassSchedule section in generatedSchedules)
        {
            if (!DoesScheduleConflict(section, selectedSchedules.Values))
            {
                chosenSection = section;
                break;
            }
        }

        if (chosenSection != null)
        {
            // Instantiate a draggable object for the course and assign it to the grid
            GameObject draggableCourse = Instantiate(courseButtonPrefab, scheduleGridParent);
            TMP_Text buttonText = draggableCourse.GetComponentInChildren<TMP_Text>();
            buttonText.text = $"{course.courseCode}\n{course.courseTitle} ({course.units}u)";

            selectedSchedules[course.courseCode] = chosenSection;
            totalUnits += course.units;
            UpdateVisualSchedule();
            UpdateStatusText();
        }
        else
        {
            Debug.LogWarning($"No non-conflicting section found for {course.courseCode}!");
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
        int startSlot = (startMinuteTotal - 450) / 30;

        int durationSlots = schedule.DurationMinutes / 30;

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

        conflictText.text = CheckForConflicts() ? "❌ Schedule Conflict!" : "✅ No Conflicts";
    }

    private bool CheckForConflicts()
    {
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
        registrationCanvas.SetActive(false);
    }

    private void ClearSelections()
    {
        selectedSchedules.Clear();
        totalUnits = 0;
        ClearScheduleGridHighlights();
        UpdateStatusText();
    }
}