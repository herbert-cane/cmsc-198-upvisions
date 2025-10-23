using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TextMeshProUGUI))]
public class HyperlinkHandler : MonoBehaviour, IPointerClickHandler
{
    private TextMeshProUGUI pText;
    private Canvas pCanvas;
    private Camera pCamera;
    
    // Get your manager script
    private EncyclopediaManager encyclopediaManager; 

    void Start()
    {
        pText = GetComponent<TextMeshProUGUI>();
        pCanvas = GetComponentInParent<Canvas>();
        pCamera = pCanvas.worldCamera;
        
        // Find the manager in the scene
        encyclopediaManager = Object.FindFirstObjectByType<EncyclopediaManager>(); 
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Check if a link was clicked
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(pText, eventData.position, pCamera);

        if (linkIndex != -1)
        {
            // A link was clicked!
            TMP_LinkInfo linkInfo = pText.textInfo.linkInfo[linkIndex];
            string linkID = linkInfo.GetLinkID();
            
            // Tell the manager to open the entry for this ID
            if (encyclopediaManager != null)
            {
                encyclopediaManager.DisplayEntry(linkID);
            }
        }
    }
}