using System;
using System.Collections.Generic;
using UnityEngine;

public class ClassSchedulerPuzzleGenerator : MonoBehaviour
{
    public int Seed = 12345;
    public int MaxSectionsPerCoreLec = 2;
    public int MaxSectionsPerCoreLab = 2;
    public int MaxSectionsPerGE = 5;
    public int CoreLectureSlots = 30;
    public int CoreLaboratorySlots = 15;
    public int GESlots = 30;
    public int NSTPSlots = 200; // No maximum
    public int UnderloadLimit = 15;
    public int OverloadLimit = 18;
    public Player player; 
    public CourseSubjectDatabase courseSubjectData;
    private System.Random rng;


    void Start()
    {
        rng = new System.Random(Seed);
    }

    public List<CourseSubjectData> GeneratePuzzleForSemester(string yearLevel, string semester)
    {
        // Fetch the courses for the selected year level and semester
        List<CourseSubjectData> semesterCourses = GetCoursesForSemester(yearLevel, semester);

        // Generate the schedule for each course
        foreach (var course in semesterCourses)
        {
            GenerateSchedulesForCourse(course); // Generate schedules dynamically
        }

        LogGeneratedOfferings(semesterCourses);
        return semesterCourses;
    }

    public List<CourseSubjectData> GetCoursesForSemester(string yearLevel, string semester)
    {
        // Format the year and semester into the format expected by `semesterEligible`
        string formattedSemester = GetFormattedSemester(yearLevel, semester);

        // Fetch courses for the formatted semester
        List<CourseSubjectData> courses = new List<CourseSubjectData>();

        // Loop through all available courses and add the ones that match the semester
        foreach (var semesterCourses in courseSubjectData.CommunicationAndMediaStudies.Values)
        {
            foreach (var course in semesterCourses)
            {
                if (course.semesterEligible == formattedSemester)
                {
                    courses.Add(course); // Add matching courses
                }
            }
        }

        return courses;
    }

    // Utility function to format year level and semester into "1Y1S", "2Y1S", etc.
    public string GetFormattedSemester(string yearLevel, string semester)
    {
        string year = yearLevel switch
        {
            "First Year" => "1Y",
            "Second Year" => "2Y",
            "Third Year" => "3Y",
            "Fourth Year" => "4Y",
            _ => "UnknownYear"
        };

        string sem = semester switch
        {
            "First Semester" => "1S",
            "Second Semester" => "2S",
            _ => "UnknownSemester"
        };

        return $"{year}{sem}";
    }

    public List<ClassSchedule> GenerateSchedulesForCourse(CourseSubjectData course)
    {
        List<ClassSchedule> generated = new List<ClassSchedule>();

        int numSections;
        int slotsPerSection;
        ScheduleType scheduleType;

        // Determine number of sections and slots based on course type
        switch (course.semesterEligible)
        {
            case "Core":
            case "Major":
                if (course.lablec.Contains("LB")) // Has Lab
                {
                    numSections = rng.Next(1, MaxSectionsPerCoreLec + 1);
                    slotsPerSection = CoreLectureSlots;
                    scheduleType = ScheduleType.Lecture;
                    for (int i = 0; i < numSections; i++)
                    {
                        ClassSchedule lecSched = GenerateRandomSchedule(course.courseCode, scheduleType, course.units, slotsPerSection, false);
                        generated.Add(lecSched);
                    }

                    numSections = rng.Next(1, MaxSectionsPerCoreLab + 1);
                    slotsPerSection = CoreLaboratorySlots;
                    scheduleType = ScheduleType.Laboratory;
                    for (int i = 0; i < numSections; i++)
                    {
                        ClassSchedule labSched = GenerateRandomSchedule(course.courseCode, scheduleType, course.units, slotsPerSection, true);
                        generated.Add(labSched);
                    }
                }
                else // No Lab
                {
                    numSections = rng.Next(1, MaxSectionsPerCoreLec + 1);
                    slotsPerSection = CoreLectureSlots;
                    scheduleType = ScheduleType.Lecture;
                    for (int i = 0; i < numSections; i++)
                    {
                        ClassSchedule sched = GenerateRandomSchedule(course.courseCode, scheduleType, course.units, slotsPerSection, false);
                        generated.Add(sched);
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
                    ClassSchedule lecSched = GenerateRandomSchedule(course.courseCode, scheduleType, course.units, slotsPerSection, false);
                    generated.Add(lecSched);
                }
                break;

            default: // Other categories
                numSections = rng.Next(1, 3);
                slotsPerSection = 30; // Default slot size
                scheduleType = ScheduleType.Lecture;
                for (int i = 0; i < numSections; i++)
                {
                    ClassSchedule lecSched = GenerateRandomSchedule(course.courseCode, scheduleType, course.units, slotsPerSection, false);
                    generated.Add(lecSched);
                }
                break;
        }

        return generated;
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

    private void LogGeneratedOfferings(List<CourseSubjectData> courses)
    {
        Debug.Log($"Generated {courses.Count} course offerings");
        foreach (var course in courses)
        {
            Debug.Log($"\n{course.courseCode} - {course.courseTitle} ({course.units} units) [{course.semesterEligible}]");
        }
    }
}
