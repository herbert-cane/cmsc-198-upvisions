// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;
// using System.Collections;
// using System.Collections.Generic;

// public class RuntimeGridBuilder : MonoBehaviour
// {
//     public bool buildOnStart = true;
//     public GameObject timeSlotPrefab; // Prefab for time slots
//     public GameObject scheduleGridPanel; // The main panel holding the schedule grid
//     public Transform gridContainer; // Container for the grid items (time slots)
//     public int dayCount = 6; // 6 days (Monday to Saturday)
//     public int timeSlotsPerDay = 12; // 12 slots (7:30 AM to 6:00 PM)
    
//     private GameObject[,] scheduleGridSlots; // Store the time slot references

//     void Start()
//     {
//         if (buildOnStart)
//         {
//             StartCoroutine(BuildGridDelayed());
//         }
//     }

//     IEnumerator BuildGridDelayed()
//     {
//         // Wait one frame to ensure everything is loaded
//         yield return null;
//         BuildScheduleGrid();
//     }

//     public void BuildScheduleGrid()
//     {
//         // Ensure scheduleGridPanel is assigned and not null
//         if (scheduleGridPanel == null)
//         {
//             Debug.LogError("ScheduleGridPanel is not assigned in the Inspector!");
//             return;
//         }

//         // Ensure gridContainer is assigned and not null
//         gridContainer = scheduleGridPanel.transform.Find("GridContainer");
//         if (gridContainer == null)
//         {
//             Debug.LogError("GridContainer not found in ScheduleGridPanel!");
//             return;
//         }

//         // Ensure timeSlotPrefab is assigned and not null
//         if (timeSlotPrefab == null)
//         {
//             Debug.LogError("TimeSlotPrefab is not assigned in the Inspector!");
//             return;
//         }

//         // Set up the `GridContainer`'s `RectTransform` to ensure it stretches within the parent
//         RectTransform gridContainerRect = gridContainer.GetComponent<RectTransform>();
//         gridContainerRect.anchorMin = Vector2.zero;
//         gridContainerRect.anchorMax = Vector2.one;
//         gridContainerRect.offsetMin = Vector2.zero;  // Reset margins
//         gridContainerRect.offsetMax = Vector2.zero;  // Reset margins

//         // Add a GridLayoutGroup to the GridContainer if it's not there already
//         GridLayoutGroup gridLayout = gridContainer.GetComponent<GridLayoutGroup>();
//         if (gridLayout == null)
//         {
//             gridLayout = gridContainer.gameObject.AddComponent<GridLayoutGroup>();
//         }

//         // Set grid layout properties for proper positioning
//         gridLayout.cellSize = new Vector2(100, 40); // Adjust cell size as needed
//         gridLayout.spacing = new Vector2(2, 2); // Spacing between cells
//         gridLayout.padding = new RectOffset(10, 10, 10, 10); // Padding for the grid container
//         gridLayout.startCorner = GridLayoutGroup.Corner.UpperLeft; // Start from the top left corner
//         gridLayout.startAxis = GridLayoutGroup.Axis.Horizontal; // Arrange horizontally (Days as columns)
//         gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount; // Constrain to a fixed number of columns (7 days + time header)
//         gridLayout.constraintCount = dayCount + 1; // 6 days + 1 time column

//         // Initialize the 2D array for schedule slots
//         scheduleGridSlots = new GameObject[dayCount, timeSlotsPerDay];

//         // Step 1: Create day headers (Monday to Saturday)
//         BuildDayHeaders();

//         // Step 2: Create time labels (7:30 AM to 6:00 PM)
//         BuildTimeLabelsColumn();

//         // Step 3: Create time slot buttons for each day and time (inside gridContainer)
//         BuildTimeSlotButtons();

//         Debug.Log("Schedule grid built at runtime!");
//     }

//     private void BuildDayHeaders()
//     {
//         string[] days = { "MON", "TUE", "WED", "THU", "FRI", "SAT" };

//         // Create day headers in the first row
//         for (int day = 0; day < dayCount; day++)
//         {
//             string dayName = days[day];
//             GameObject header = new GameObject($"Header-{dayName}");
//             header.transform.SetParent(gridContainer);

//             // Add a TextMeshPro component for the header
//             TMP_Text headerText = header.AddComponent<TextMeshProUGUI>();
//             headerText.text = dayName;
//             headerText.fontSize = 14;
//             headerText.alignment = TextAlignmentOptions.Center;

//             // Set RectTransform for positioning (optional)
//             RectTransform headerRect = header.GetComponent<RectTransform>();
//             headerRect.sizeDelta = new Vector2(100, 40); // Adjust size as needed
//         }
//     }

//     private void BuildTimeLabelsColumn()
//     {
//         // Create a separate container for time labels
//         GameObject timeLabelContainer = new GameObject("TimeLabelContainer");
//         timeLabelContainer.transform.SetParent(gridContainer);

//         // Create time labels below the days
//         for (int time = 0; time < timeSlotsPerDay; time++)
//         {
//             int hour = 7 + (time / 2); // Start at 7 AM
//             int minute = (time % 2) * 30; // 00 or 30
//             string timeLabel = $"{hour:00}:{minute:00}";

//             // Create time label for each row
//             GameObject timeLabelObj = new GameObject($"TimeLabel-{timeLabel}");
//             timeLabelObj.transform.SetParent(timeLabelContainer.transform);

//             // Add TMP_Text for the time label
//             TMP_Text timeLabelText = timeLabelObj.AddComponent<TextMeshProUGUI>();
//             timeLabelText.text = timeLabel;
//             timeLabelText.fontSize = 12;
//             timeLabelText.alignment = TextAlignmentOptions.Center;

//             // Set RectTransform for positioning (optional)
//             RectTransform timeLabelRect = timeLabelObj.GetComponent<RectTransform>();
//             timeLabelRect.sizeDelta = new Vector2(100, 40); // Adjust size as needed
//         }
//     }

//     private void BuildTimeSlotButtons()
//     {
//         if (timeSlotPrefab == null)
//         {
//             Debug.LogError("TimeSlotPrefab is still null!");
//             return;
//         }

//         // Create time slots (below time labels)
//         for (int day = 0; day < dayCount; day++)
//         {
//             for (int time = 0; time < timeSlotsPerDay; time++)
//             {
//                 GameObject slot = Instantiate(timeSlotPrefab, gridContainer);
//                 slot.name = $"Slot-{(DayOfWeek)day}-{time}";

//                 // Store the slot in the grid array for later access
//                 scheduleGridSlots[day, time] = slot;

//                 // Set slot button's position (You can adjust layout as needed)
//                 RectTransform slotRect = slot.GetComponent<RectTransform>();
//                 slotRect.localPosition = new Vector3(0, 0, 0); // Center slot within grid container

//                 // Add functionality when a time slot is clicked
//                 Button button = slot.GetComponent<Button>();
//                 button.onClick.AddListener(() => OnTimeSlotClicked(day, time));
//             }
//         }
//     }

//     private void OnTimeSlotClicked(int day, int time)
//     {
//         // Handle the click on a time slot (e.g., display course info, add to selected schedule)
//         Debug.Log($"Time slot clicked: Day-{(DayOfWeek)day} Time-{(time / 2) + 7}:{(time % 2) * 30}");
//     }

//     public void UpdateGridWithCourseSchedule(List<ClassSchedule> schedules)
//     {
//         // Clear the grid before updating
//         ClearScheduleGridHighlights();

//         // Iterate over the schedules and update the grid
//         foreach (var schedule in schedules)
//         {
//             // Color the slots according to the course's schedule (highlighting)
//             Color scheduleColor = schedule.Type == ScheduleType.Lecture ? Color.blue : Color.green;
//             Vector2Int[] gridPositions = ConvertScheduleToGridPositions(schedule);

//             foreach (Vector2Int pos in gridPositions)
//             {
//                 if (pos.x < dayCount && pos.y < timeSlotsPerDay)
//                 {
//                     Image slotImage = scheduleGridSlots[pos.x, pos.y].GetComponent<Image>();
//                     slotImage.color = scheduleColor;

//                     TMP_Text slotText = scheduleGridSlots[pos.x, pos.y].GetComponentInChildren<TMP_Text>();
//                     slotText.text = schedule.CourseNo;
//                 }
//             }
//         }
//     }

//     private Vector2Int[] ConvertScheduleToGridPositions(ClassSchedule schedule)
//     {
//         List<Vector2Int> positions = new List<Vector2Int>();
//         int startMinuteTotal = schedule.StartHour * 60 + schedule.StartMinute;
//         int startSlot = (startMinuteTotal - 450) / 30; // 7:30 AM = 450 minutes, each slot is 30 mins

//         int durationSlots = schedule.DurationMinutes / 30;

//         // For split schedules, we need to add positions for both days
//         if (schedule.IsSplit)
//         {
//             AddSlotsForDay((int)schedule.Day1, startSlot, durationSlots, positions);
//             AddSlotsForDay((int)schedule.Day2, startSlot, durationSlots, positions);
//         }
//         else
//         {
//             AddSlotsForDay((int)schedule.Day1, startSlot, durationSlots, positions);
//         }
//         return positions.ToArray();
//     }

//     private void AddSlotsForDay(int day, int startSlot, int durationSlots, List<Vector2Int> positions)
//     {
//         for (int i = 0; i < durationSlots; i++)
//         {
//             positions.Add(new Vector2Int(day, startSlot + i));
//         }
//     }

//     private void ClearScheduleGridHighlights()
//     {
//         for (int day = 0; day < dayCount; day++)
//         {
//             for (int time = 0; time < timeSlotsPerDay; time++)
//             {
//                 scheduleGridSlots[day, time].GetComponent<Image>().color = Color.white;
//                 scheduleGridSlots[day, time].GetComponentInChildren<TMP_Text>().text = "";
//             }
//         }
//     }
// }