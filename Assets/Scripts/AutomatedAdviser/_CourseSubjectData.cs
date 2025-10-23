using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CourseSubjectDataSO", menuName = "Data/Course Subject Data")]
public class CourseSubjectDataSO : ScriptableObject
{
    [SerializeField]
    private List<AcademicProgram> programs = new List<AcademicProgram>();

    // ✅ Retrieves all subjects for given program, year, and semester
    public List<CourseSubject> GetSubjects(string programName, int year, int semester)
    {
        AcademicProgram program = programs.Find(p => p.programName == programName);
        if (program == null)
        {
            Debug.LogWarning($"Program not found: {programName}");
            return new List<CourseSubject>();
        }

        string key = $"{year}-{semester}";
        if (program.semesterSubjects.TryGetValue(key, out List<CourseSubject> subjects))
        {
            return subjects;
        }

        Debug.LogWarning($"No subjects found for {programName}, Year {year}, Sem {semester}");
        return new List<CourseSubject>();
    }

    // Add a subject to the program based on the year and semester
    public void AddSubjectToProgram(string programName, int year, int semester, CourseSubject subject)
    {
        AcademicProgram program = programs.Find(p => p.programName == programName);
        if (program == null)
        {
            Debug.LogWarning($"Program not found: {programName}");
            return;
        }

        string key = $"{year}-{semester}";
        if (!program.semesterSubjects.ContainsKey(key))
        {
            program.semesterSubjects[key] = new List<CourseSubject>();
        }

        program.semesterSubjects[key].Add(subject);
    }

    // Add a program
    public void AddProgram(AcademicProgram program)
    {
        programs.Add(program);
    }

    // Clear all subjects
    public void Clear()
    {
        foreach (var program in programs)
        {
            program.semesterSubjects.Clear();
        }
    }

    // Return all available programs
    public List<string> GetProgramNames()
    {
        List<string> names = new List<string>();
        foreach (var p in programs)
            names.Add(p.programName);
        return names;
    }
}

[Serializable]
public class AcademicProgram
{
    public string programName;

    // Unity cannot serialize Dictionary directly — use helper list
    [SerializeField]
    private List<SemesterSubjects> semesters = new List<SemesterSubjects>();

    public Dictionary<string, List<CourseSubject>> semesterSubjects = new();

    // Convert serialized list to runtime dictionary
    public void Initialize()
    {
        semesterSubjects.Clear();
        foreach (var sem in semesters)
        {
            string key = $"{sem.year}-{sem.semester}";
            semesterSubjects[key] = new List<CourseSubject>(sem.subjects);
        }
    }
}

[Serializable]
public class SemesterSubjects
{
    public int year;
    public int semester;
    public List<CourseSubject> subjects = new List<CourseSubject>();
}

[Serializable]
public class CourseSubject
{
    public string code;
    public string title;
    public int units;
    public string label;
    public string prerequisites; // Comma-separated course codes
    public string semesterEligible; // e.g., "1", "2", "Both"
}