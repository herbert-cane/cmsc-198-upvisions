// using UnityEditor;
// using UnityEngine;

// public class CourseSubjectDataEditor : EditorWindow
// {
//     [MenuItem("Tools/Populate Course Subject Data")]
//     public static void ShowWindow()
//     {
//         EditorWindow.GetWindow(typeof(CourseSubjectDataEditor));
//     }

//     private void OnGUI()
//     {
//         if (GUILayout.Button("Populate CourseSubjectDataSO"))
//         {
//             PopulateCourseSubjectData();
//         }
//     }

//     private void PopulateCourseSubjectData()
//     {
//         // Fetch the ScriptableObject
//         CourseSubjectDataSO courseData = AssetDatabase.LoadAssetAtPath<CourseSubjectDataSO>("Assets/Path/To/Your/CourseSubjectDataSO.asset");

//         if (courseData == null)
//         {
//             Debug.LogError("CourseSubjectDataSO not found.");
//             return;
//         }

//         // Clear existing data before repopulating
//         courseData.Clear();

//         // Fetch the data for courses, replace this with your existing CourseSubjectData setup or file
//         List<CourseSubjectData> subjects = GetCourseSubjects();  // Replace with your actual data source

//         foreach (var subject in subjects)
//         {
//             // Add to the program list (adjust based on your actual data structure)
//             courseData.AddCourseToProgram(subject);
//         }

//         // Save changes
//         EditorUtility.SetDirty(courseData);
//         AssetDatabase.SaveAssets();
//         Debug.Log("CourseSubjectDataSO populated successfully.");
//     }

//     // This is a placeholder, replace with your data fetching logic
//     private List<CourseSubjectData> GetCourseSubjects()
//     {
//         // Replace with actual data population logic
//         List<CourseSubjectData> subjects = new List<CourseSubjectData>
//         {
//             new CourseSubjectData("CMS_101", "Introduction to Media Communication", 3, "3LC", "None", "1Y1S"),
//             new CourseSubjectData("CMS_102", "Communication and Media Theories", 3, "3LC", "CMS_101", "2Y1S"),
//             // Add more courses here as needed
//         };

//         return subjects;
//     }
// }