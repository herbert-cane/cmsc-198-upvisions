using UnityEngine;
using System.Collections.Generic;
using System;

public class ScholarshipManager : MonoBehaviour
{
    [Header("Player's Active Scholarships")]
    public List<ScholarshipSO> activeScholarships = new List<ScholarshipSO>();

    [Header("References")]
    public MoneyManager moneyManager;
    public TimeManager timeManager;

    private void Start()
    {
        // Find references if not assigned
        if (moneyManager == null) moneyManager = FindFirstObjectByType<MoneyManager>();
        if (timeManager == null) timeManager = FindFirstObjectByType<TimeManager>();

        // Subscribe to the time Tick
        if (timeManager != null)
        {
            timeManager.OnHourTick += CheckForPayouts;
        }
    }

    private void OnDestroy()
    {
        if (timeManager != null) timeManager.OnHourTick -= CheckForPayouts;
    }

    // Called every in-game hour by TimeManager
    public void CheckForPayouts(DateTime currentTime)
    {
        // We only want to process payments at a specific time, e.g., 8:00 AM
        if (currentTime.Hour != 8) return;

        foreach (var scholarship in activeScholarships)
        {
            bool shouldPay = false;

            switch (scholarship.frequency)
            {
                case ReleaseFrequency.Weekly:
                    // Pay if it's Monday
                    if (currentTime.DayOfWeek == DayOfWeek.Monday) shouldPay = true;
                    break;

                case ReleaseFrequency.Monthly:
                    // Pay if it's the 1st of the month
                    if (currentTime.Day == 1) shouldPay = true;
                    break;
            }

            if (shouldPay)
            {
                ReleaseFunds(scholarship);
            }
        }
    }

    public void GrantScholarship(ScholarshipSO newScholarship)
    {
        if (!activeScholarships.Contains(newScholarship))
        {
            activeScholarships.Add(newScholarship);
            Debug.Log($"Scholarship Granted: {newScholarship.scholarshipName}");

            // If it's One-Time, pay immediately and don't add to the recurring list
            if (newScholarship.frequency == ReleaseFrequency.OneTime)
            {
                ReleaseFunds(newScholarship);
                activeScholarships.Remove(newScholarship);
            }
        }
    }

    private void ReleaseFunds(ScholarshipSO scholarship)
    {
        if (moneyManager != null)
        {
            moneyManager.ModifyMoney(scholarship.amount);
            Debug.Log($"<color=green>STIPEND RECEIVED:</color> {scholarship.scholarshipName} (+{scholarship.amount})");
            // Optional: Add a UI popup here notification
        }
    }
}