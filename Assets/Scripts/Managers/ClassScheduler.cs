using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum DayOfWeek { Monday, Tuesday, Wednesday, Thursday, Friday, Saturday }
public enum ScheduleType { Lecture, Laboratory }

public class ClassSchedule : MonoBehaviour
{
    public string CourseNo { get; private set; }
    public ScheduleType Type;
    public DayOfWeek Day1;
    public DayOfWeek Day2; // For classes split over two days
    public int StartHour; // 24-hour format (7 to 18)
    public int StartMinute;
    public int DurationMinutes; // Total minutes for the session
    public bool IsSplit; // True if the class is split over two days (e.g., Mon/Thu)
    public int SlotsAvailable;

    public ClassSchedule(string courseNo, ScheduleType type, DayOfWeek day, int startHour, int startMinute, int durationMinutes, int slots)
    {
        CourseNo = courseNo;
        Type = type;
        Day1 = day;
        Day2 = day;
        StartHour = startHour;
        StartMinute = startMinute;
        DurationMinutes = durationMinutes;
        IsSplit = false;
        SlotsAvailable = slots;
    }

    public ClassSchedule(string courseNo, ScheduleType type, DayOfWeek day1, DayOfWeek day2, int startHour, int startMinute, int durationPerSessionMinutes, int slots)
    {
        CourseNo = courseNo;
        Type = type;
        Day1 = day1;
        Day2 = day2;
        StartHour = startHour;
        StartMinute = startMinute;
        DurationMinutes = durationPerSessionMinutes;
        IsSplit = true;
        SlotsAvailable = slots;
    }

    public bool ConflictsWith(ClassSchedule other)
    {
        if (this.IsSplit)
        {
            bool conflictDay1 = CheckTimeConflict(this.Day1, this.StartHour, this.StartMinute, this.DurationMinutes,
                                                  other.Day1, other.StartHour, other.StartMinute, other.GetEffectiveDuration(other.Day1));
            bool conflictDay2 = CheckTimeConflict(this.Day2, this.StartHour, this.StartMinute, this.DurationMinutes,
                                                  other.Day2, other.StartHour, other.StartMinute, other.GetEffectiveDuration(other.Day2));
            return conflictDay1 || conflictDay2;
        }
        else
        {
            bool conflictThisDay = CheckTimeConflict(this.Day1, this.StartHour, this.StartMinute, this.DurationMinutes,
                                                    other.Day1, other.StartHour, other.StartMinute, other.GetEffectiveDuration(this.Day1));
            bool conflictOtherDay2 = other.IsSplit ? CheckTimeConflict(this.Day1, this.StartHour, this.StartMinute, this.DurationMinutes,
                                                                     other.Day2, other.StartHour, other.StartMinute, other.GetEffectiveDuration(this.Day1)) : false;
            return conflictThisDay || conflictOtherDay2;
        }
    }

    private bool CheckTimeConflict(DayOfWeek dayA, int startHourA, int startMinuteA, int durationMinutesA,
                                  DayOfWeek dayB, int startHourB, int startMinuteB, int durationMinutesB)
    {
        if (dayA != dayB) return false;

        int startTimeA = startHourA * 60 + startMinuteA;
        int endTimeA = startTimeA + durationMinutesA;

        int startTimeB = startHourB * 60 + startMinuteB;
        int endTimeB = startTimeB + durationMinutesB;

        return startTimeA < endTimeB && endTimeA > startTimeB;
    }

    public int GetEffectiveDuration(DayOfWeek day)
    {
        if (!IsSplit) return DurationMinutes;
        return (day == Day1 || day == Day2) ? DurationMinutes : 0;
    }

    public override string ToString()
    {
        string typeStr = Type == ScheduleType.Laboratory ? "[LAB]" : "[LEC]";
        if (IsSplit)
        {
            int endHour = StartHour + (DurationMinutes / 60);
            int endMinute = (StartMinute + (DurationMinutes % 60));
            if (endMinute >= 60)
            {
                endHour += 1;
                endMinute -= 60;
            }
            return $"{typeStr} {Day1} & {Day2}, {StartHour:00}:{StartMinute:00}-{endHour:00}:{endMinute:00} (Slots: {SlotsAvailable})";
        }
        else
        {
            int endHour = StartHour + (DurationMinutes / 60);
            int endMinute = (StartMinute + (DurationMinutes % 60));
            if (endMinute >= 60)
            {
                endHour += 1;
                endMinute -= 60;
            }
            return $"{typeStr} {Day1}, {StartHour:00}:{StartMinute:00}-{endHour:00}:{endMinute:00} (Slots: {SlotsAvailable})";
        }
    }
}
