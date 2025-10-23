// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;

// public class FilterButton : MonoBehaviour
// {
//     public ScheduleType filterType; // e.g., "Core", "Elective", "GE"
//     public bool isActive = true;
    
//     private Button button;
//     private TMP_Text text;
//     private Outline outline;
    
//     void Start()
//     {
//         button = GetComponent<Button>();
//         text = GetComponentInChildren<TMP_Text>();
//         outline = GetComponent<Outline>();
        
//         button.onClick.AddListener(ToggleFilter);
//         UpdateVisuals();
//     }
    
//     void ToggleFilter()
//     {
//         isActive = !isActive;
//         UpdateVisuals();
        
//         // Check if RegistrationUIController instance exists
//         if (RegistrationUIController.Instance != null)
//         {
//             RegistrationUIController.Instance.UpdateFilter(filterType, isActive);
//         }
//         else
//         {
//             Debug.LogWarning("RegistrationUIController instance not found!");
//         }
//     }
    
//     void UpdateVisuals()
//     {
//         if (isActive)
//         {
//             // Active state - bright blue with outline
//             button.image.color = new Color(0.3f, 0.6f, 1f, 1f);
//             if (outline != null) outline.effectColor = new Color(0.1f, 0.3f, 0.8f, 1f);
//             text.color = Color.white;
//         }
//         else
//         {
//             // Inactive state - gray without outline
//             button.image.color = new Color(0.6f, 0.6f, 0.6f, 0.6f);
//             if (outline != null) outline.effectColor = new Color(0.4f, 0.4f, 0.4f, 0.5f);
//             text.color = new Color(0.3f, 0.3f, 0.3f, 0.7f);
//         }
//     }
// }