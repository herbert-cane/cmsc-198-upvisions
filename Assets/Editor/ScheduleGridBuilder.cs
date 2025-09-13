using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using System.Collections.Generic;

public class ScheduleGridBuilder : MonoBehaviour
{
    [MenuItem("Tools/Build Schedule Grid (Constrained Size)")]
    public static void BuildConstrainedScheduleGrid()
    {
        GameObject scheduleGridPanel = GameObject.Find("ScheduleGridPanel");
        if (scheduleGridPanel == null)
        {
            Debug.LogError("ScheduleGridPanel not found!");
            return;
        }

        ClearChildren(scheduleGridPanel.transform);

        // FIRST: Set the ScheduleGridPanel to fit within its parent
        SetupPanelConstraints(scheduleGridPanel);

        // Create a container that will hold the scroll view and enforce size limits
        GameObject gridContainer = CreateUIObject("GridContainer", scheduleGridPanel);
        SetupGridContainer(gridContainer);

        // Create the scroll view inside the container
        GameObject content = CreateScrollView(gridContainer);
        
        // Build the grid content
        BuildGridContent(content);

        Debug.Log("Constrained schedule grid built successfully!");
    }

    private static void SetupPanelConstraints(GameObject panel)
    {
        RectTransform rt = panel.GetComponent<RectTransform>();
        
        // Set panel to stretch within its parent but with margins
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = new Vector2(10, 10);  // Left/Bottom margins
        rt.offsetMax = new Vector2(-10, -120); // Right/Top margins (reserve space for SelectionPanel)
        
        // Add a layout element to prevent infinite expansion
        LayoutElement layoutElem = panel.GetComponent<LayoutElement>();
        if (layoutElem == null) layoutElem = panel.AddComponent<LayoutElement>();
        layoutElem.flexibleHeight = 1; // Can grow but within constraints
    }

    private static void SetupGridContainer(GameObject container)
    {
        // Set container to fill the ScheduleGridPanel
        RectTransform rt = container.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        // Add vertical layout to stack elements properly
        VerticalLayoutGroup layout = container.AddComponent<VerticalLayoutGroup>();
        layout.padding = new RectOffset(0, 0, 0, 0);
        layout.spacing = 0;
        layout.childAlignment = TextAnchor.UpperCenter;
        layout.childForceExpandWidth = true;
        layout.childForceExpandHeight = false;

        // Constrain the container height
        LayoutElement layoutElem = container.AddComponent<LayoutElement>();
        layoutElem.preferredHeight = 400; // Fixed height for the grid area
        layoutElem.flexibleHeight = 0; // Cannot grow beyond preferred height
    }

    private static GameObject CreateScrollView(GameObject parent)
    {
        GameObject scrollView = CreateUIObject("ScheduleScrollView", parent);
        
        // Set scroll view to expand width but fixed height
        RectTransform scrollRT = scrollView.GetComponent<RectTransform>();
        scrollRT.anchorMin = new Vector2(0, 0);
        scrollRT.anchorMax = new Vector2(1, 1);
        scrollRT.offsetMin = Vector2.zero;
        scrollRT.offsetMax = Vector2.zero;

        // Add Scroll Rect
        ScrollRect scrollRect = scrollView.AddComponent<ScrollRect>();
        scrollRect.horizontal = true;
        scrollRect.vertical = true;
        scrollRect.movementType = ScrollRect.MovementType.Clamped;
        scrollRect.inertia = true;
        scrollRect.decelerationRate = 0.135f;

        // Create Viewport
        GameObject viewport = CreateUIObject("Viewport", scrollView);
        RectTransform viewportRT = viewport.GetComponent<RectTransform>();
        viewportRT.anchorMin = Vector2.zero;
        viewportRT.anchorMax = Vector2.one;
        viewportRT.offsetMin = Vector2.zero;
        viewportRT.offsetMax = Vector2.zero;
        
        viewport.AddComponent<Image>().color = new Color(1, 1, 1, 0.5f);
        viewport.AddComponent<Mask>().showMaskGraphic = false;

        // Create Content
        GameObject content = CreateUIObject("Content", viewport);
        RectTransform contentRT = content.GetComponent<RectTransform>();
        contentRT.anchorMin = new Vector2(0, 1);
        contentRT.anchorMax = new Vector2(0, 1);
        contentRT.pivot = new Vector2(0, 1);

        // Connect scroll rect
        scrollRect.viewport = viewportRT;
        scrollRect.content = contentRT;

        return content;
    }

    private static void BuildGridContent(GameObject content)
    {
        // Use a grid layout for better control
        GridLayoutGroup gridLayout = content.AddComponent<GridLayoutGroup>();
        gridLayout.cellSize = new Vector2(60, 25); // Smaller cells
        gridLayout.spacing = new Vector2(2, 1);
        gridLayout.padding = new RectOffset(5, 5, 5, 5);
        gridLayout.startCorner = GridLayoutGroup.Corner.UpperLeft;
        gridLayout.startAxis = GridLayoutGroup.Axis.Horizontal;
        gridLayout.childAlignment = TextAnchor.UpperLeft;
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = 7; // Time column + 6 days

        // Build grid cells
        BuildGridCells(content);

        // Content size fitter
        ContentSizeFitter sizeFitter = content.AddComponent<ContentSizeFitter>();
        sizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        sizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
    }

    private static void BuildGridCells(GameObject parent)
    {
        // Create header row first
        CreateCell(parent, "TimeHeader", "", 25, new Color(0.8f, 0.8f, 0.8f));
        
        string[] days = { "MON", "TUE", "WED", "THU", "FRI", "SAT" };
        foreach (string day in days)
        {
            CreateCell(parent, $"{day}Header", day, 25, 
                      (day == "SAT") ? new Color(0.8f, 0.9f, 1f) : new Color(0.8f, 0.8f, 0.8f));
        }

        // Create time slots (reduced number for better fit)
        string[] timeSlots = {
            "7:30", "8:00", "8:30", "9:00", "9:30", "10:00", "10:30", "11:00",
            "11:30", "12:00", "13:00", "14:00", "15:00", "16:00", "17:00"
        };

        foreach (string time in timeSlots)
        {
            // Time label cell
            CreateCell(parent, $"Time{time}", time, 20, new Color(0.9f, 0.9f, 0.9f));

            // Day cells for this time slot
            for (int i = 0; i < 6; i++)
            {
                CreateCell(parent, $"{days[i]}_{time}", "", 20, new Color(1f, 1f, 1f, 0.3f));
            }
        }
    }

    private static void CreateCell(GameObject parent, string name, string text, float height, Color color)
    {
        GameObject cell = CreateUIObject(name, parent);
        
        // Add layout element for size
        LayoutElement layout = cell.AddComponent<LayoutElement>();
        layout.minHeight = height;
        
        // Add background
        Image bg = cell.AddComponent<Image>();
        bg.color = color;

        // Add border
        Outline outline = cell.AddComponent<Outline>();
        outline.effectColor = new Color(0, 0, 0, 0.1f);
        outline.effectDistance = new Vector2(1, 1);

        // Add text if provided
        if (!string.IsNullOrEmpty(text))
        {
            GameObject textObj = CreateUIObject("Text", cell);
            TMP_Text textComp = textObj.AddComponent<TextMeshProUGUI>();
            textComp.text = text;
            textComp.fontSize = 9;
            textComp.alignment = TextAlignmentOptions.Center;
            textComp.color = Color.black;

            // Center text
            RectTransform textRT = textObj.GetComponent<RectTransform>();
            textRT.anchorMin = Vector2.zero;
            textRT.anchorMax = Vector2.one;
            textRT.offsetMin = Vector2.zero;
            textRT.offsetMax = Vector2.zero;
        }

        cell.AddComponent<TimeSlotTooltip>();
    }

    // Helper methods remain the same...
    private static GameObject CreateUIObject(string name, GameObject parent)
    {
        GameObject obj = new GameObject(name, typeof(RectTransform));
        obj.transform.SetParent(parent.transform);
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = Vector3.zero;
        return obj;
    }

    private static void ClearChildren(Transform parent)
    {
        int childCount = parent.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            GameObject child = parent.GetChild(i).gameObject;
            if (child != null)
            {
                Undo.DestroyObjectImmediate(child);
            }
        }
    }
}