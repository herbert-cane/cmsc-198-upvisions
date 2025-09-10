using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum DayOfWeek { Monday, Tuesday, Wednesday, Thursday, Friday, Saturday }
public enum ScheduleType { Lecture, Laboratory }

[System.Serializable]
public class ClassSchedule
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

    // Constructor for a class held on a single day (e.g., 3-hour lab)
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

    // Constructor for a class split over two days (e.g., 1.5h Lec on Mon and 1.5h on Thu)
    public ClassSchedule(string courseNo,ScheduleType type, DayOfWeek day1, DayOfWeek day2, int startHour, int startMinute, int durationPerSessionMinutes, int slots)
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
        // Check if the schedules overlap on any day and time
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

[System.Serializable]
public class CourseOffering
{
    public string CourseNo;
    public string CourseTitle;
    public int Units;
    public string CourseType; // "Core", "Major", "GE Core", "GE Elective", "Elective", "PE", "NSTP"
    public bool HasLaboratory;
    public List<ClassSchedule> AvailableSchedules;

    public CourseOffering(string courseNo, string title, int units, string courseType, bool hasLab = false)
    {
        CourseNo = courseNo;
        CourseTitle = title;
        Units = units;
        CourseType = courseType;
        HasLaboratory = hasLab;
        AvailableSchedules = new List<ClassSchedule>();
    }

    public int GetTotalWeeklyMinutes()
    {
        // If has lab, double the time (e.g., 3 units = 3 hrs lecture + 3 hrs lab = 360 min)
        return HasLaboratory ? Units * 60 * 2 : Units * 60;
    }
}

public class ClassSchedulerPuzzleGenerator : MonoBehaviour
{
    public int Seed = 12345;
    public int MaxSectionsPerCoreLec = 2;
    public int MaxSectionsPerCoreLab = 2;
    public int MaxSectionsPerGE = 5;

    // Maximum slots per section
    public int CoreLectureSlots = 30;
    public int CoreLaboratorySlots = 15;
    public int GESlots = 30;
    public int NSTPSlots = int.MaxValue; // No maximum

    // Unit limits
    public int UnderloadLimit = 15;
    public int OverloadLimit = 18;

    private System.Random rng;

    void Start()
    {
        rng = new System.Random(Seed);
        List<CourseOffering> semesterOfferings = GeneratePuzzleForSemester("Second Year", "First Semester");
        SimulatePlayerRegistration(semesterOfferings);
    }

    public List<CourseOffering> GeneratePuzzleForSemester(string yearLevel, string semester)
    {
        List<CourseOffering> semesterCourses = GetCoursesForSemester(yearLevel, semester);
        foreach (var course in semesterCourses)
        {
            GenerateSchedulesForCourse(course);
        }
        LogGeneratedOfferings(semesterCourses);
        return semesterCourses;
    }

    private void GenerateSchedulesForCourse(CourseOffering course)
    {
        int numSections;
        int slotsPerSection;
        ScheduleType scheduleType;

        // Determine number of sections and slots based on course type
        switch (course.CourseType)
        {
            case "Core":
            case "Major":
                // For courses with lab, we need to generate both Lec and Lab sections
                if (course.HasLaboratory)
                {
                    // Generate Lecture Sections
                    numSections = rng.Next(1, MaxSectionsPerCoreLec + 1);
                    slotsPerSection = CoreLectureSlots;
                    scheduleType = ScheduleType.Lecture;
                    for (int i = 0; i < numSections; i++)
                    {
                        ClassSchedule lecSched = GenerateRandomSchedule(course.CourseNo, scheduleType, course.Units, slotsPerSection, false);
                        course.AvailableSchedules.Add(lecSched);
                    }

                    // Generate Laboratory Sections
                    numSections = rng.Next(1, MaxSectionsPerCoreLab + 1);
                    slotsPerSection = CoreLaboratorySlots;
                    scheduleType = ScheduleType.Laboratory;
                    for (int i = 0; i < numSections; i++)
                    {
                        ClassSchedule labSched = GenerateRandomSchedule(course.CourseNo, scheduleType, course.Units, slotsPerSection, true);
                        course.AvailableSchedules.Add(labSched);
                    }
                }
                else
                {
                    numSections = rng.Next(1, MaxSectionsPerCoreLec + 1);
                    slotsPerSection = CoreLectureSlots;
                    scheduleType = ScheduleType.Lecture;
                    for (int i = 0; i < numSections; i++)
                    {
                        ClassSchedule sched = GenerateRandomSchedule(course.CourseNo, scheduleType, course.Units, slotsPerSection, false);
                        course.AvailableSchedules.Add(sched);
                    }
                }
                break;

            case "GE Core":
            case "GE Elective":
                numSections = rng.Next(3, MaxSectionsPerGE + 1);
                slotsPerSection = GESlots;
                scheduleType = ScheduleType.Lecture;
                for (int i = 0; i < numSections; i++)
                {
ClassSchedule lecSched = GenerateRandomSchedule(course.CourseNo, scheduleType, course.Units, slotsPerSection, false);

                }
                break;

            case "NSTP":
                numSections = 1; // NSTP usually has fixed schedules
                slotsPerSection = NSTPSlots;
                scheduleType = ScheduleType.Lecture;
                ClassSchedule nstpSched = GenerateRandomSchedule(course.CourseNo, scheduleType, course.Units, slotsPerSection, false);
                course.AvailableSchedules.Add(nstpSched);
                break;

            default: // PE, Elective, etc.
                numSections = rng.Next(1, 3);
                slotsPerSection = 30; // Default slot size
                scheduleType = ScheduleType.Lecture;
                for (int i = 0; i < numSections; i++)
                {
                    ClassSchedule lecSched = GenerateRandomSchedule(course.CourseNo, scheduleType, course.Units, slotsPerSection, false);
                    course.AvailableSchedules.Add(lecSched);
                }
                break;
        }
    }

    private ClassSchedule GenerateRandomSchedule(string courseNo, ScheduleType type, int units, int slots, bool isLab)
    {
        int totalWeeklyMinutes = units * 60;
        bool makeSplit = !isLab && (units >= 2) && (rng.NextDouble() > 0.4);

        int durationPerSession = makeSplit ? totalWeeklyMinutes / 2 : totalWeeklyMinutes;

        int startHour = rng.Next(7, 17);
        int startMinute = (rng.Next(0, 2) == 0) ? 0 : 30;

        if (makeSplit)
        {
            DayOfWeek day1 = (DayOfWeek)rng.Next(0, 5);
            int dayGap = 2 + rng.Next(0, 2);
            DayOfWeek day2 = (DayOfWeek)(((int)day1 + dayGap) % 5);
            if (day2 == DayOfWeek.Saturday) day2 = DayOfWeek.Friday;

            return new ClassSchedule(courseNo, type, day1, day2, startHour, startMinute, durationPerSession, slots);
        }
        else
        {
            DayOfWeek day = (DayOfWeek)rng.Next(0, 6);
            return new ClassSchedule(courseNo, type, day, startHour, startMinute, totalWeeklyMinutes, slots);
        }
    }

    private void SimulatePlayerRegistration(List<CourseOffering> offerings)
    {
        // This simulates a player trying to build a schedule
        List<ClassSchedule> selectedSchedules = new List<ClassSchedule>();
        int totalUnits = 0;

        Debug.Log("=== PLAYER'S SCHEDULE ATTEMPT ===");

        foreach (var course in offerings)
        {
            // Player selects first available section that doesn't conflict
            foreach (var section in course.AvailableSchedules.OrderBy(s => s.SlotsAvailable))
            {
                bool conflicts = selectedSchedules.Any(selected => selected.ConflictsWith(section));
                if (!conflicts && section.SlotsAvailable > 0)
                {
                    selectedSchedules.Add(section);
                    totalUnits += course.Units;
                    Debug.Log($"✅ ADDED: {course.CourseNo} - {section.ToString()}");
                    break;
                }
            }
        }

        // Check unit limits
        Debug.Log($"Total Units Registered: {totalUnits}");
        if (totalUnits < UnderloadLimit) Debug.LogWarning("❌ UNDERLOAD: Less than 15 units.");
        if (totalUnits > OverloadLimit) Debug.LogWarning("❌ OVERLOAD: More than 18 units.");
        if (totalUnits >= UnderloadLimit && totalUnits <= OverloadLimit) Debug.Log("✅ Unit load is within acceptable limits.");
    }

    private void LogGeneratedOfferings(List<CourseOffering> courses)
    {
        Debug.Log($"Generated {courses.Count} course offerings");
        foreach (var course in courses)
        {
            Debug.Log($"\n{course.CourseNo} - {course.CourseTitle} ({course.Units} units) [{course.CourseType}]");
            foreach (var sched in course.AvailableSchedules)
            {
                Debug.Log($"  {sched.ToString()}");
            }
        }
    }

    private List<CourseOffering> GetCoursesForSemester(string yearLevel, string semester)
    {
        List<CourseOffering> courses = new List<CourseOffering>();

        if (yearLevel == "Second Year" && semester == "First Semester")
        {
            courses.Add(new CourseOffering("Free Elective", "Free Elective", 3, "Elective"));
            courses.Add(new CourseOffering("GE Core 4", "General Education Core 4", 3, "GE Core"));
            courses.Add(new CourseOffering("GE Elective 2", "General Education Elective 2", 3, "GE Elective"));
            courses.Add(new CourseOffering("Math 54", "Calculus II", 4, "Core"));
            courses.Add(new CourseOffering("Stat 121", "Probability Theory I", 3, "Major"));
            courses.Add(new CourseOffering("PE", "Physical Education", 2, "PE"));
            courses.Add(new CourseOffering("NSTP", "National Service Training Program", 3, "NSTP"));
        }
        else
        {
            // Default example
            courses.Add(new CourseOffering("CMSC 11", "Intro to Computer Science", 3, "Core", true));
            courses.Add(new CourseOffering("GE Core 1", "General Education Core 1", 3, "GE Core"));
        }
        return courses;
    }
}