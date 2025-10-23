using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;

public class ScheduleGridManager : MonoBehaviour
{
    [Header("Grid Setup")]
    public GameObject cellPrefab;       // Prefab for grid cells
    public Transform gridParent;        // Parent to hold the grid cells
    public TMP_Text dayTextPrefab;      // TMP_Text for day names
    public TMP_Text timeSlotTextPrefab; // TMP_Text for time slots
    public int rows = 18;               // Number of rows (time slots)
    public int columns = 6;             // Number of columns (days of the week)
    public GameObject subjectPrefab;    // Reference to the subject prefab
    public Transform subjectListParent; // Parent to hold subject prefabs

    private GameObject[,] gridCells;    // To store grid cells
    private List<ClassSchedule> classSchedules = new();
    private GameObject draggedSubject;
    private RectTransform draggedRect;

    void Start()
    {
        GenerateGrid();
    }

    // Generate the time slots grid (rows for time, columns for days)
    void GenerateGrid()
    {
        string[] days = { "MON", "TUE", "WED", "THU", "FRI", "SAT" };
        // Ensure gridCells is initialized
        if (gridCells == null)
        {
            gridCells = new GameObject[columns, rows];
        }

        // Ensure subjectListParent is assigned
        if (subjectListParent == null)
        {
            Debug.LogError("subjectListParent is not assigned in the Inspector!");
            return;
        }

        // Ensure cellPrefab is assigned
        if (cellPrefab == null)
        {
            Debug.LogError("cellPrefab is not assigned in the Inspector!");
            return;
        }

        // Create grid headers (Days)
        for (int day = 0; day < columns; day++)
        {
            string dayName = days[day];
            GameObject header = new GameObject($"Header-{dayName}");
            header.transform.SetParent(subjectListParent);

            // Add a TextMeshPro component for the header
            TMP_Text headerText = header.AddComponent<TextMeshProUGUI>();
            headerText.text = dayName;
            headerText.fontSize = 14;
            headerText.alignment = TextAlignmentOptions.Center;

            // Set RectTransform for positioning (optional)
            RectTransform headerRect = header.GetComponent<RectTransform>();
            headerRect.sizeDelta = new Vector2(200, 40); // Adjust size as needed
        }

        // Create time slots
        for (int time = 0; time < rows; time++)
        {
            int hour = 7 + (time / 2); // Start at 7 AM
            int minute = (time % 2) * 30; // 00 or 30
            string timeLabel = $"{hour:00}:{minute:00}";

            // Create time label (for 7:30 AM, 8:30 AM, etc.)
            GameObject timeLabelObj = new GameObject($"TimeLabel-{timeLabel}");
            timeLabelObj.AddComponent<TextMeshProUGUI>().text = timeLabel;
            timeLabelObj.transform.SetParent(subjectListParent);

            // Adjust RectTransform for time label positioning
            RectTransform timeLabelRect = timeLabelObj.GetComponent<RectTransform>();
            timeLabelRect.sizeDelta = new Vector2(200, 40); // Adjust size as needed

            // Create the time slot objects below the time labels
            for (int day = 0; day < columns; day++)
            {
                GameObject slot = Instantiate(cellPrefab, subjectListParent);
                slot.name = $"Slot-{(DayOfWeek)day}-{timeLabel}";
                gridCells[day, time] = slot;

                // Add trigger or functionality (e.g., tooltips, course details on hover)
                EventTrigger trigger = slot.AddComponent<EventTrigger>();
            }
        }
    }

    private void BuildDayHeaders()
    {
        string[] days = { "MON", "TUE", "WED", "THU", "FRI", "SAT" };

        // Create headers for each day (Mon-Sat) in the first row
        for (int day = 0; day < columns; day++)
        {
            string dayName = days[day];
            GameObject header = new GameObject($"Header-{dayName}");
            header.transform.SetParent(subjectListParent);

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
        timeLabelContainer.transform.SetParent(subjectListParent);

        // Create the time labels (7:30, 8:00, etc.)
        for (int time = 0; time < rows; time++)
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
        for (int time = 0; time < rows; time++)
        {
            for (int day = 0; day < columns; day++)
            {
                GameObject slot = Instantiate(cellPrefab, subjectListParent);
                slot.name = $"Slot-{(DayOfWeek)day}-{time}";

                // Store the slot in the grid array for later access
                gridCells[day, time] = slot;

                // Set slot button's position (You can adjust layout as needed)
                RectTransform slotRect = slot.GetComponent<RectTransform>();
                slotRect.localPosition = new Vector3(0, 0, 0); // Center slot within grid container

                // Add functionality when a time slot is clicked
                Button button = slot.GetComponent<Button>();
                // button.onClick.AddListener(() => OnTimeSlotClicked(day, time));
            }
        }
    }

    // // Handle when a cell is clicked (where a subject will be placed)
    // void OnCellClick(int rowIndex, int colIndex)
    // {
    //     if (draggedSubject == null) return;

    //     // Calculate the clip time (either 00th or 30th minute)
    //     int startTime = rowIndex < rows / 2 ? rowIndex * 60 : (rowIndex * 60) + 30;

    //     // Get the current dragged subject details (you can use your ClassSchedule model here)
    //     ClassSchedule newSchedule = new ClassSchedule(
    //         subjectCode: draggedSubject.name,
    //         subjectTitle: "Sample Title", // Replace with actual title
    //         units: 3,                     // Replace with actual units
    //         section: "A",                 // Replace with actual section
    //         day: colIndex switch
    //         {
    //             0 => "Mon",
    //             1 => "Tue",
    //             2 => "Wed",
    //             3 => "Thu",
    //             4 => "Fri",
    //             _ => "Sat"
    //         },
    //         timeSlot: $"{(startTime / 60):D2}:{(startTime % 60):D2}-{((startTime + 90) / 60):D2}:{((startTime + 90) % 60):D2}"
    //     );

    //     // Check for conflicts
    //     if (CheckForConflicts(newSchedule))
    //     {
    //         Debug.LogWarning("Conflict detected! Cannot place this schedule.");
    //         return;
    //     }

    //     // Assign the dragged prefab to the clicked cell
    //     draggedSubject.transform.SetParent(gridParent);
    //     draggedSubject.transform.position = gridCells[rowIndex, colIndex].transform.position;

    //     // Add the schedule to the list
    //     classSchedules.Add(newSchedule);

    //     // Reset the dragged subject
    //     draggedSubject = null;
    // }

    // // Allow dragging the subject prefab
    // public void StartDrag(GameObject subject)
    // {
    //     draggedSubject = subject;
    //     draggedRect = subject.GetComponent<RectTransform>();
    // }

    // // Update dragged prefab position based on the mouse position
    // void Update()
    // {
    //     if (draggedSubject != null)
    //     {
    //         draggedRect.position = Input.mousePosition;
    //     }
    // }

    // // Check if the newly placed subject conflicts with existing ones
    // bool CheckForConflicts(ClassSchedule newSchedule)
    // {
    //     foreach (var existingSchedule in classSchedules)
    //     {
    //         if (existingSchedule.ConflictsWith(newSchedule))
    //         {
    //             return true;
    //         }
    //     }
    //     return false;
    // }
}
