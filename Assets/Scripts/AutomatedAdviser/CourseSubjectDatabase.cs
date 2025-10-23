using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class CourseSubjectDatabase : MonoBehaviour
{
    public static CourseSubjectDatabase Instance;
    // Division of Social Sciences and Humanities
    public Dictionary<string, List<CourseSubjectData>> CommunicationAndMediaStudies;
    public Dictionary<string, List<CourseSubjectData>> CommunityDevelopment;
    public Dictionary<string, List<CourseSubjectData>> Economics;
    public Dictionary<string, List<CourseSubjectData>> History;
    public Dictionary<string, List<CourseSubjectData>> Literature;
    public Dictionary<string, List<CourseSubjectData>> PoliticalScience;
    public Dictionary<string, List<CourseSubjectData>> Psychology;
    public Dictionary<string, List<CourseSubjectData>> Sociology;

    // Division of Physical Sciences and Mathematics
    public Dictionary<string, List<CourseSubjectData>> AppliedMathematics;
    public Dictionary<string, List<CourseSubjectData>> ComputerScience;
    public Dictionary<string, List<CourseSubjectData>> Chemistry;
    public Dictionary<string, List<CourseSubjectData>> Statistics;

    // Division of Biological Sciences
    public Dictionary<string, List<CourseSubjectData>> Biology;
    public Dictionary<string, List<CourseSubjectData>> PublicHealth;

    // College of Fisheries and Ocean Sciences
    public Dictionary<string, List<CourseSubjectData>> Fisheries;

    // College of Management
    public Dictionary<string, List<CourseSubjectData>> Accountancy;
    public Dictionary<string, List<CourseSubjectData>> Management;
    public Dictionary<string, List<CourseSubjectData>> BusinessAdministration;

    // School of Technology
    public Dictionary<string, List<CourseSubjectData>> ChemicalEngineering;
    public Dictionary<string, List<CourseSubjectData>> FoodTechnology;

    // GE Courses
    public Dictionary<string, List<CourseSubjectData>> GeneralEducationCore;
    public Dictionary<string, List<CourseSubjectData>> GeneralEducationElectives;

    // PE Courses
    public Dictionary<string, List<CourseSubjectData>> PhysicalEducation;

    // NSTP Courses
    public Dictionary<string, List<CourseSubjectData>> NationalServiceTrainingProgram;

    // Free Electives
    public Dictionary<string, List<CourseSubjectData>> FreeElectives;

    void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Ensure it persists across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate if any
        }

        InitializeData(); // Initialize the data
    }

    private void InitializeData()
    {
        CommunicationAndMediaStudies = new Dictionary<string, List<CourseSubjectData>>()
        {
            // **First Year First Semester**
            { "First Year First Semester", new List<CourseSubjectData> {
                new CourseSubjectData("GEC_1", "General Education Core 1", 3,"", "", "1Y1S"),
                new CourseSubjectData("GEC_2", "General Education Core 2", 3,"", "", "1Y1S"),
                new CourseSubjectData("GEC_E_1", "General Education Elective 1", 3,"", "", "1Y1S"),
                new CourseSubjectData("CMS_21", "Foundations of Media Writing", 3,"3LC", "", "1Y1S"),
                new CourseSubjectData("CMS_31", "Effective Oral Skills for Media Communication", 3,"3LC", "", "1Y1S"),
                new CourseSubjectData("CMS_100", "History of Media Communication", 3,"3LC", "", "1Y1S"),
                new CourseSubjectData("NSTP_1", "National Service Training Program 1", 0,"2LC", "", "1Y1S"),
                new CourseSubjectData("PE_1", "Foundations of Physical Fitness", 0,"2LC", "", "1Y1S")
            }},

            // **First Year Second Semester**
            { "First Year Second Semester", new List<CourseSubjectData> {
                new CourseSubjectData("CMS_E_1", "CMS Elective 1", 3,"", "", "1Y2S"),
                new CourseSubjectData("CMS_E_2", "CMS Elective 2", 3,"", "", "1Y2S"),
                new CourseSubjectData("GEC_3", "General Education Core 3", 3,"", "", "1Y2S"),
                new CourseSubjectData("GEC_4", "General Education Core 4", 3,"", "", "1Y2S"),
                new CourseSubjectData("GEC_E_2", "General Education Elective 2", 3,"", "", "1Y2S"),
                new CourseSubjectData("CMS_101", "Introduction to Media Communication", 3,"3LC", "", "1Y2S"),
                new CourseSubjectData("NSTP_2", "National Service Training Program 2", 0,"2LC", "", "1Y2S"),
                new CourseSubjectData("PE_2", "Foundations of Physical Fitness", 0,"2LC", "", "1Y2S")
            } },

            // **Second Year First Semester**
            { "Second Year First Semester", new List<CourseSubjectData> {
                new CourseSubjectData("CMS_E_3", "CMS Elective 3", 3,"3LC", "", "2Y1S"),
                new CourseSubjectData("CMS_E_4", "CMS Elective 4", 3,"3LC", "", "2Y1S"),
                new CourseSubjectData("GEC_5", "General Education Core 5", 3,"3LC", "", "2Y1S"),
                new CourseSubjectData("GEC_6", "General Education Core 6", 3,"3LC", "", "2Y1S"),
                new CourseSubjectData("GEC_7", "General Education Core 7", 3,"3LC", "", "2Y1S"),
                new CourseSubjectData("CMS_102", "Communication and Media Theories", 3,"3LC", "CMS_101", "2Y1S"),
                new CourseSubjectData("PE_3", "Foundations of Physical Fitness", 0,"2LC", "", "2Y1S")
            }},

            // **Second Year Second Semester**
            { "Second Year Second Semester", new List<CourseSubjectData> {
                new CourseSubjectData("CMS_E_5", "CMS Elective 3", 3,"3LC", "", "2Y1S"),
                new CourseSubjectData("GE_E_3", "General Education Elective 3", 3,"", "", "2Y2S"),
                new CourseSubjectData("GE_E_4", "General Education Elective 4", 3,"", "", "2Y2S"),
                new CourseSubjectData("GE_E_5", "General Education Elective 5", 3,"", "", "2Y2S"),
                new CourseSubjectData("CMS_103", "Media-Related Laws and Codes", 3,"3LC", "CMS_101", "2Y2S"),
                new CourseSubjectData("CMS_105", "Technologies in Media Communication", 3,"3LC", "CMS_100", "2Y2S"),
                new CourseSubjectData("PE_4", "Foundations of Physical Fitness", 0,"2LC", "", "2Y2S")
            }},

            // **Third Year First Semester**
            { "Third Year First Semester", new List<CourseSubjectData> {
                new CourseSubjectData("CMS_E_6", "CMS Elective 6", 3,"3LC", "", "3Y1S"),
                new CourseSubjectData("CMS_E_7", "CMS Elective 7", 3,"3LC", "", "3Y1S"),
                new CourseSubjectData("CMS_104", "Media and the Community", 3,"3LC", "CMS_102", "3Y1S"),
                new CourseSubjectData("CMS_107", "Fundamentals of Communication Planning", 3,"3LC", "CMS_102", "3Y1S"),
                new CourseSubjectData("CMS_190", "Digital Media Composition for Production", 3,"3LC", "", "3Y1S"),
            }},

            // **Third Year Second Semester**
            { "Third Year Second Semester", new List<CourseSubjectData> {
                new CourseSubjectData("FE_1", "Free Elective", 3,"3LC","", "3Y2S"),
                new CourseSubjectData("CMS_E_8", "CMS Elective 8", 3,"3LC", "", "3Y2S"),
                new CourseSubjectData("CMS_E_9", "CMS Elective 9", 3,"3LC", "","3Y2S"),
                new CourseSubjectData("CMS_110", "Development Media", 3,"3LC", "","3Y2S"),
                new CourseSubjectData("CMS_194", "Media Appreciation and Criticism", 3,"3LC", "CMS_102", "3Y2S"),
                new CourseSubjectData("CMS_197", "Communication Research Methods", 3,"3LC", "CMS_102, JuniorStanding", "3Y1S")
            }},

            // **Third Year Midyear**
            { "Third Year Midyear", new List<CourseSubjectData> {
                new CourseSubjectData("CMS 195", "Media Internship", 3,"3LC", "", "3YMid")
            }},

            // **Fourth Year First Semester**
            { "Fourth Year First Semester", new List<CourseSubjectData> {
                new CourseSubjectData("FE_2", "Free Elective", 3,"3LC", "", "4Y1S"),
                new CourseSubjectData("CMS_E_9", "CMS Elective 9", 3,"3LC", "", "4Y1S"),
                new CourseSubjectData("CMS_192", "Media Writing for Specific Purposes", 3,"3LC", "CMS_102", "4Y1S"),
                new CourseSubjectData("CMS_198", "Contemporary Issues in Media Communication", 3,"3LC", "CMS_195", "4Y1S"),
                new CourseSubjectData("CMS_199.1", "Research or Creative Work in Communication and Media Studies I", 3,"3LC", "CMS_197, SeniorStanding", "4Y1S"),
            }},

            // **Fourth Year Second Semester**
            { "Fourth Year Second Semester", new List<CourseSubjectData> {
                new CourseSubjectData("FE_3", "Free Elective", 3,"3LC", "", "4Y2S"),
                new CourseSubjectData("CMS_E_10", "CMS Elective 10", 3,"3LC", "", "4Y2S"),
                new CourseSubjectData("COMM_11", "Professional Communication", 3,"3LC", "", "4Y2S"),
                new CourseSubjectData("CMS_199.2", "Research or Creative Work in Communication and Media Studies II", 3,"3LC", "CMC_199.1, SeniorStanding", "4Y2S"),
                new CourseSubjectData("PI_100", "The Life and Works of Jose Rizal", 3,"3LC", "", "4Y2S")
            }},

            // CMS Electives
            { "CMS Electives", new List<CourseSubjectData> {
                new CourseSubjectData("CMS_11", "Professional Communication", 3,"3LC", "COMM_10", ""),
                new CourseSubjectData("CMS_12", "Technical Communication", 3,"3LC", "COMM_10", ""),
                new CourseSubjectData("CMS_21", "Dynamics of Human Communication", 3,"3LC", "None", ""),
                new CourseSubjectData("CMS_22", "Foundations of Media Writing", 3,"3LC", "None", ""),
                new CourseSubjectData("CMS_31", "Effective Oral Skills for Media Communication", 3,"3LC", "None", ""),
                new CourseSubjectData("CMS_100", "History of Media Communication", 3,"3LC", "None", ""),
                new CourseSubjectData("CMS_101", "Introduction to Media Communication", 3,"3LC", "None", ""),
                new CourseSubjectData("CMS_102", "Communication and Media Theories", 3,"3LC", "CMS_101", ""),
                new CourseSubjectData("CMS_103", "Media-Related Laws and Codes", 3,"3LC", "CMS_101", ""),
                new CourseSubjectData("CMS_104", "Media and the Community", 3,"3LC", "CMS_102", ""),
                new CourseSubjectData("CMS_105", "Technologies in Media Communication", 3,"3LC", "CMS_100", ""),
                new CourseSubjectData("CMS_107", "Fundamentals of Communication Planning", 3,"3LC", "CMS_102", ""),
                new CourseSubjectData("CMS_110", "Development Media", 3,"3LC", "CMS_100", ""),
                new CourseSubjectData("CMS_111", "Audio Procedures and Techniques", 3,"1LC-2LB", "None", ""),
                new CourseSubjectData("CMS_112", "Radio Speech and Performance", 3,"1LC-2LB", "CMS_31", ""),
                new CourseSubjectData("CMS_113", "Radio Writing", 3,"1LC-2LB", "CMS_21", ""),
                new CourseSubjectData("CMS_115", "Radio Production and Direction", 3,"2LC-3LB", "CMS_113", ""),
                new CourseSubjectData("CMS_118", "Community Radio", 3,"1LC-2LB", "CMS_115", ""),
                new CourseSubjectData("CMS_121", "Video Procedures and Techniques", 3,"1LC-2LB", "None", ""),
                new CourseSubjectData("CMS_122", "Television Speech and Performance", 3,"1LC-2LB", "CMS_31", ""),
                new CourseSubjectData("CMS_123", "Television Writing", 3,"1LC-2LB", "CMS_21", ""),
                new CourseSubjectData("CMS_125", "Television Production and Direction", 3,"1LC-2LB", "CMS_121", ""),
                new CourseSubjectData("CMS_128", "Broadcast News", 3,"1LC-2LB", "CMS_111, CMS_121", ""),
                new CourseSubjectData("CMS_130", "Basics of Print Journalism", 3,"1LC-2LB", "CMS_21", ""),
                new CourseSubjectData("CMS_131", "Covering and Writing the News", 3,"1LC-2LB", "CMS_130", ""),
                new CourseSubjectData("CMS_132", "Editorial, Column and Cartoon", 3,"1LC-2LB", "CMS_130", ""),
                new CourseSubjectData("CMS_133", "Feature Writing", 3,"1LC-2LB", "CMS_130", ""),
                new CourseSubjectData("CMS_134", "Procedures and Techniques in Print Media", 3,"2LC-3LB", "CMS_131", ""),
                new CourseSubjectData("CMS_135", "Contemporary and Online Publishing", 3,"1LC-2LB", "CMS_131, CMS_132", ""),
                new CourseSubjectData("CMS_136", "Specialized Reporting", 3,"1LC-2LB", "CMS_131", ""),
                new CourseSubjectData("CMS_137", "Photojournalism", 3,"1LC-2LB", "None", ""),
                new CourseSubjectData("CMS_138", "Magazine Design and Publishing", 3,"1LC-2LB", "CMS_132, CMS_134", ""),
                new CourseSubjectData("CMS_139", "Investigative and In-depth Reporting", 3,"1LC-2LB", "CMS_131", ""),
                new CourseSubjectData("CMS_154", "Program Design in Broadcasting", 3,"3LC", "CMSC_111", ""),
                new CourseSubjectData("CMS_159", "Broadcast Programming and Management", 3,"3LC", "SeniorStanding", ""),
                new CourseSubjectData("CMS_160", "Understanding Advertising", 3,"3LC", "", "4Y2S"),
                new CourseSubjectData("CMS_161", "Fundamentals of Creative Developments in Advertising", 3,"1LC-2LB", "CMS_160", ""),
                new CourseSubjectData("CMS_162", "Integrated and Interactive Advertising Strategies", 3,"1LC-2LB", "CMS_160", ""),
                new CourseSubjectData("CMS_163", "Media Planning and Evaluation", 3,"3LC", "CMS_160", ""),
                new CourseSubjectData("CMS_164", "Advocacy Advertising and Public Relations", 3,"1LC-2LB", "CMS_160", ""),
                new CourseSubjectData("CMS_165", "Print Advertising", 3,"1LC-2LB", "CMS_160", ""),
                new CourseSubjectData("CMS_166", "Broadcast Advertising", 3,"1LC-2LB", "CMS_160", ""),
                new CourseSubjectData("CMS_167", "Digital and Online Advertising", 3,"1LC-2LB", "CMS_160", ""),
                new CourseSubjectData("CMS_168", "Planning the Advertising Campaign", 3,"1LC-2LB", "CMS_163", ""),
                new CourseSubjectData("CMS_169", "Implementing the Advertising Campaign", 3,"1LC-2LB", "CMS_168", ""),
                new CourseSubjectData("CMS_170", "Introduction to World Cinema", 3,"3LC", "", ""),
                new CourseSubjectData("CMS_171", "Introduction to Philippine Cinema", 3,"3LC", "", ""),
                new CourseSubjectData("CMS_172", "Approaches to Film Theory and Criticism", 3,"3LC", "", ""),
                new CourseSubjectData("CMS_173", "Gender Studies in Cinema", 3,"3LC", "", ""),
                new CourseSubjectData("CMS_174", "Screenwriting", 3,"1LC-2LB", "", ""),
                new CourseSubjectData("CMS_175", "Cinematography, Editing and Sound", 3,"1LC-2LB", "CMS_174", ""),
                new CourseSubjectData("CMS_176", "Producing", 3,"1LC-2LB", "CMS_174", ""),
                new CourseSubjectData("CMS_177", "Directing the Narrative", 3,"1LC-2LB", "CMS_174", ""),
                new CourseSubjectData("CMS_178", "Directing the Non-Narrative", 3,"1LC-2LB", "CMS_174", ""),
                new CourseSubjectData("CMS_179", "Special Topics in Cinema", 3,"1LC-2LB", "", ""),
                new CourseSubjectData("CMS_190", "Digital Media Composition for Production", 3,"3LC", "", ""),
                new CourseSubjectData("CMS_192", "Media Writing for Specific Purposes", 3,"3LC", "CMS_102", ""),
                new CourseSubjectData("CMS_194", "Media Appreciation and Criticism", 3,"3LC", "CMS_102", ""),
                new CourseSubjectData("CMS_195", "Media Internship", 3,"200 hours", "JuniorStanding", ""),
                new CourseSubjectData("CMS_197", "Communication Research Methods", 3,"3LC", "CMS_102, JuniorStanding", ""),
                new CourseSubjectData("CMS_198", "Contemporary Issues in Media Communication", 3,"3LC", "CMS_195", ""),
            }}

        };

    }
}

[Serializable]
public class CourseSubjectData
{
    public string code;
    public string title;
    public int units;
    public string label; // e.g. "3LC" or "1LC-2LB"

    [Tooltip("Comma-separated prerequisite course codes (e.g. CMS_101, CMS_102)")]
    public string prerequisites;

    [Tooltip("Semester eligibility restriction (e.g. 4Y2S)")]
    public string semesterEligible;

    /// <summary>
    /// Returns a list of prerequisite codes split by commas.
    /// </summary>
    public List<string> GetPrerequisites()
    {
        if (string.IsNullOrWhiteSpace(prerequisites)) return new List<string>();
        string[] split = prerequisites.Split(',');
        List<string> list = new List<string>();
        foreach (var p in split)
            list.Add(p.Trim());
        return list;
    }

    public override string ToString()
    {
        return $"{code}: {title} ({units} units)";
    }


    public CourseSubjectData(string courseCode, string courseTitle, int units, string label, string prerequisites, string semesterEligible)
    {
        this.code = courseCode;
        this.title = courseTitle;
        this.units = units;
        this.label = label;
        this.prerequisites = prerequisites;
        this.semesterEligible = semesterEligible;
    }
}

