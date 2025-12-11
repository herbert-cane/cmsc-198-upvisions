using UnityEngine;
using TMPro; // Needed for TextMeshPro

public class MoneyManager : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private float currentBalance = 1000f; // Starting money

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI moneyText;

    void Start()
    {
        UpdateMoneyUI();
    }

    // Call this to Add money (e.g., Scholarship: ModifyMoney(500))
    // Call this to Subtract money (e.g., Buying: ModifyMoney(-50))
    public bool ModifyMoney(float amount)
    {
        // If trying to spend more than we have, fail the transaction
        if (amount < 0 && (currentBalance + amount < 0))
        {
            Debug.Log("Insufficient funds!");
            return false;
        }

        currentBalance += amount;
        UpdateMoneyUI();
        return true;
    }

    private void UpdateMoneyUI()
    {
        if (moneyText != null)
        {
            // Format to 2 decimal places (e.g., "₱ 1,500.00")
            moneyText.text = currentBalance.ToString("N2");
        }
    }
}