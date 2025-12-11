using UnityEngine;
using System; // Needed for DateTime
using TMPro;

public class TimeManager : MonoBehaviour
{
    [Header("Date Settings")]
    public int startYear = 2025; // Set the XXXX year here
    
    // We use C#'s DateTime for robust calendar handling
    private DateTime currentGameTime;

    public event Action<DateTime> OnHourTick;

    [Header("Time Flow Config")]
    // "Increments by 5 minutes every 1 minute in real world"
    public float realSecondsPerTick = 60f; 
    public int gameMinutesPerTick = 5;
    
    [Tooltip("Set higher to test faster (e.g., 60 = 1 sec real time takes 1 min to tick)")]
    public float debugSpeedMultiplier = 1f; 

    private float timer;

    [Header("UI References")]
    public TextMeshProUGUI dateText;
    public TextMeshProUGUI timeText;

    [Header("Events")]
    public MoneyManager moneyManager; // Reference to add Stipend

    void Tick()
    {
        DateTime previousTime = currentGameTime;
        currentGameTime = currentGameTime.AddMinutes(gameMinutesPerTick);

        // Check if we entered a new hour (e.g., 7:55 -> 8:00)
        if (currentGameTime.Hour != previousTime.Hour)
        {
            // Broadcast to all listeners (ScholarshipManager, HungerSystem, etc.)
            OnHourTick?.Invoke(currentGameTime); // <--- ADD THIS
        }
        
        UpdateUI();
    }

    void Start()
    {
        // Initialize Date: Aug 25, XXXX at 8:00 AM
        currentGameTime = new DateTime(startYear, 8, 25, 8, 0, 0);
        
        UpdateUI();
    }

    void Update()
    {
        // Accumulate time
        timer += Time.deltaTime * debugSpeedMultiplier;

        // Check if 1 real minute (60 seconds) has passed
        if (timer >= realSecondsPerTick)
        {
            Tick();
            timer = 0f;
        }
    }


    void CheckForWeeklyEvents()
    {
        // Example: If it's Monday (DayOfWeek.Monday) AND exactly 8:00 AM
        if (currentGameTime.DayOfWeek == DayOfWeek.Monday && 
            currentGameTime.Hour == 8 && 
            currentGameTime.Minute == 0)
        {
            Debug.Log("Scholarship Stipend Received!");
            if(moneyManager != null) moneyManager.ModifyMoney(2000f); 
        }
    }

    void UpdateUI()
    {
        // Format Date: "Mon, Aug. 25, 2025"
        // ddd = Abbr Day (Mon), MMM = Abbr Month (Aug), dd = Day (25), yyyy = Year
        if (dateText != null)
            dateText.text = currentGameTime.ToString("ddd, MMM. dd, yyyy");

        // Format Time: "08:05 AM"
        if (timeText != null)
            timeText.text = currentGameTime.ToString("hh:mm tt");
    }
}