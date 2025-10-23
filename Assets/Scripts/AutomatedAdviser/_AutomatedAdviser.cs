using System;
using System.Collections.Generic;
using UnityEngine;

public class AutomatedAdviser : MonoBehaviour
{
    [Header("References")]
    public Player player;
    public CourseSubjectDataSO courseData;
    public CourseSubjectDatabase courseDatabase;

    [Header("Generated Data (Read-Only)")]
    public List<ClassSchedule> generatedSchedules = new List<ClassSchedule>();

    // Event for notifying when schedules are generated
    public event Action OnSchedulesGenerated;

    void Start()
    {
        if (player == null)
            player = FindFirstObjectByType<Player>();

        // Initialize Course Data
        InitializePrograms();

        // Auto-generate schedule for current player
        GenerateScheduleForPlayer();
    }

    private void InitializePrograms()
    {
        foreach (var programName in courseData.GetProgramNames())
        {
            var program = GetProgram(programName);
            if (program != null)
                program.Initialize();
        }
    }

    private AcademicProgram GetProgram(string programName)
    {
        var programsField = typeof(CourseSubjectDataSO)
            .GetField("programs", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (programsField == null) return null;

        var list = programsField.GetValue(courseData) as List<AcademicProgram>;
        return list?.Find(p => p.programName == programName);
    }

    public void GenerateScheduleForPlayer()
    {
        generatedSchedules.Clear();

        string program = player.playerStats.academicProgram;
        int year = player.playerStats.yearLevel;
        int semester = player.playerStats.semester;

        List<CourseSubject> subjects = courseData.GetSubjects(program, year, semester);
        if (subjects.Count == 0)
        {
            Debug.LogWarning($"No subjects found for {program} Y{year}S{semester}");
            return;
        }

        List<CourseSubjectData> courseDataSubjects = ConvertToCourseSubjectDataList(subjects);
        List<CourseSubjectData> eligible = FilterEligibleSubjects(courseDataSubjects, player, new List<string>());

        foreach (var subject in eligible)
        {
            ClassSchedule schedule = GenerateRandomSchedule(subject);
            generatedSchedules.Add(schedule);
        }

        Debug.Log($"Generated {generatedSchedules.Count} eligible schedules for {program} Year {year} Sem {semester}");

        // Trigger event after schedule generation
        OnSchedulesGenerated?.Invoke();
    }

    private List<CourseSubjectData> ConvertToCourseSubjectDataList(List<CourseSubject> courseSubjects)
    {
        List<CourseSubjectData> courseDataList = new List<CourseSubjectData>();
        foreach (var subject in courseSubjects)
        {
            courseDataList.Add(new CourseSubjectData(
                subject.code, 
                subject.title, 
                subject.units, 
                subject.label, 
                subject.prerequisites, 
                subject.semesterEligible));
        }
        return courseDataList;
    }

    private ClassSchedule GenerateRandomSchedule(CourseSubjectData subject)
    {
        string[] sections = { "A", "B", "C" };
        string[] timeSlots = { "07:30-09:00", "09:00-10:30", "10:30-12:00", "13:00-14:30", "15:00-16:30" };
        string[] days = { "Mon", "Tue", "Wed", "Thu", "Fri" };

        ClassSchedule newSchedule = new ClassSchedule
        {
            subjectCode = subject.code,
            subjectTitle = subject.title,
            units = subject.units,
            section = sections[UnityEngine.Random.Range(0, sections.Length)],
            day = days[UnityEngine.Random.Range(0, days.Length)],
            timeSlot = timeSlots[UnityEngine.Random.Range(0, timeSlots.Length)]
        };

        foreach (var existing in generatedSchedules)
        {
            if (newSchedule.ConflictsWith(existing))
            {
                return GenerateRandomSchedule(subject);
            }
        }

        return newSchedule;
    }

    public List<ClassSchedule> GetGeneratedSchedules()
    {
        return generatedSchedules;
    }

    private List<CourseSubjectData> FilterEligibleSubjects(List<CourseSubjectData> subjects, Player player, List<string> completed)
    {
        List<CourseSubjectData> eligible = new List<CourseSubjectData>();

        foreach (var subject in subjects)
        {
            if (!string.IsNullOrWhiteSpace(subject.semesterEligible) &&
                !IsSemesterEligible(subject.semesterEligible, player))
            {
                continue;
            }

            bool hasAllPrereqs = true;
            foreach (string prereq in subject.GetPrerequisites())
            {
                if (string.IsNullOrWhiteSpace(prereq)) continue;
                if (!completed.Contains(prereq))
                {
                    hasAllPrereqs = false;
                    break;
                }
            }

            if (hasAllPrereqs)
            {
                eligible.Add(subject);
            }
        }

        return eligible;
    }

    private bool IsSemesterEligible(string requirement, Player player)
    {
        try
        {
            int reqYear = int.Parse(requirement.Substring(0, 1));
            int reqSem = int.Parse(requirement.Substring(2, 1));
            return player.playerStats.yearLevel >= reqYear && player.playerStats.semester >= reqSem;
        }
        catch
        {
            return true;
        }
    }
}
