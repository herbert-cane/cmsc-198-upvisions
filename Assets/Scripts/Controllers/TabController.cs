using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;  // Import TextMeshPro namespace

public class TabController : MonoBehaviour
{
    public Image[] tabIcons; // Array of tab icons (Image components)
    public GameObject[] pages; // Array of pages corresponding to each tab
    public Sprite[] activeIcons; // Active tab sprites
    public Sprite[] inactiveIcons; // Inactive tab sprites
    public TextMeshProUGUI[] tabText; // Array of TextMeshPro components for each tab's text
    public Image[] tabImageIcons; // Array of Image components for each tab's icon
    public CanvasGroup[] pageCanvasGroups; // CanvasGroups for pages (for fade effect)

    // Tab active and inactive sprite sizes
    public float activePosX = 50f;
    public float activeWidth = 160f;
    public float inactivePosX = 80f;
    public float inactiveWidth = 240f;

    public float transitionDuration = 0.5f; // Duration of the transition

    void Start()
    {
        ActivateTab(0); // Activate the first tab by default
    }

    public void ActivateTab(int index)
    {
        for (int i = 0; i < tabIcons.Length; i++)
        {
            if (i == index)
            {
                // Start transition for active tab
                StartCoroutine(SmoothTransition(i, true));

                // Set the active tab icon sprite and color
                tabIcons[i].sprite = activeIcons[i];
                tabIcons[i].color = Color.white; // Active tab icon color
                pages[i].SetActive(true); // Show corresponding page

                // Activate the Image and deactivate the Text
                tabText[i].gameObject.SetActive(false); // Hide text
                tabImageIcons[i].gameObject.SetActive(true); // Show image

                // Adjust RectTransform for the active tab
                AdjustTabRectTransform(i, true);
            }
            else
            {
                // Start transition for inactive tab
                StartCoroutine(SmoothTransition(i, false));

                // Set the inactive tab icon sprite and color
                tabIcons[i].sprite = inactiveIcons[i];
                tabIcons[i].color = Color.gray; // Inactive tab icon color
                pages[i].SetActive(false); // Hide other pages

                // Activate the Text and deactivate the Image
                tabText[i].gameObject.SetActive(true); // Show text
                tabImageIcons[i].gameObject.SetActive(false); // Hide image

                // Adjust RectTransform for inactive tabs
                AdjustTabRectTransform(i, false);
            }
        }
    }

    // Smooth Transition for active/inactive pages (fade effect)
    private IEnumerator SmoothTransition(int index, bool isActive)
    {
        CanvasGroup canvasGroup = pageCanvasGroups[index];
        float targetAlpha = isActive ? 1f : 0f;

        float startAlpha = canvasGroup.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, (elapsedTime / transitionDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = targetAlpha; // Ensure the final alpha is set
    }

    // Adjust RectTransform properties for active/inactive states (smooth position transition)
    private void AdjustTabRectTransform(int index, bool isActive)
    {
        RectTransform tabRect = tabIcons[index].GetComponent<RectTransform>();

        float startPosX = tabRect.anchoredPosition.x;
        float targetPosX = isActive ? activePosX : inactivePosX;

        float startWidth = tabRect.sizeDelta.x;
        float targetWidth = isActive ? activeWidth : inactiveWidth;

        StartCoroutine(SmoothRectTransform(tabRect, startPosX, targetPosX, startWidth, targetWidth));
    }

    // Smooth position and size transition for the tab
    private IEnumerator SmoothRectTransform(RectTransform tabRect, float startPosX, float targetPosX, float startWidth, float targetWidth)
    {
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            tabRect.anchoredPosition = new Vector2(Mathf.Lerp(startPosX, targetPosX, (elapsedTime / transitionDuration)), tabRect.anchoredPosition.y);
            tabRect.sizeDelta = new Vector2(Mathf.Lerp(startWidth, targetWidth, (elapsedTime / transitionDuration)), tabRect.sizeDelta.y);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final position and size are set
        tabRect.anchoredPosition = new Vector2(targetPosX, tabRect.anchoredPosition.y);
        tabRect.sizeDelta = new Vector2(targetWidth, tabRect.sizeDelta.y);
    }
}