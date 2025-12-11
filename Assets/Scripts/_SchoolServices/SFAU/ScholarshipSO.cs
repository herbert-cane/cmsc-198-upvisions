using UnityEngine;

public enum ReleaseFrequency
{
    OneTime,    // Paid once immediately
    Weekly,     // Paid every Monday
    Monthly     // Paid on the 1st of every month
}

[CreateAssetMenu(fileName = "New Scholarship", menuName = "Systems/Scholarship")]
public class ScholarshipSO : ScriptableObject
{
    [Header("Info")]
    public string scholarshipName;
    [TextArea] public string description;

    [Header("Payment Settings")]
    public float amount;
    public ReleaseFrequency frequency;

    [Header("Requirements (Optional)")]
    public float minGWA = 1.0f; // Grade requirement to keep it
}