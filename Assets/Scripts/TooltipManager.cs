using UnityEngine;
using TMPro;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance;
    
    public GameObject tooltipPanel;
    public TMP_Text tooltipText;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        HideTooltip();
    }
    
    public void ShowTooltip(string text)
    {
        tooltipPanel.SetActive(true);
        tooltipText.text = text;
        
        // Position near mouse
        Vector2 mousePos = Input.mousePosition;
        tooltipPanel.transform.position = new Vector2(mousePos.x + 20, mousePos.y - 20);
    }
    
    public void HideTooltip()
    {
        tooltipPanel.SetActive(false);
    }
}