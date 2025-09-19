using UnityEngine;
using UnityEngine.UI;

public class TabController : MonoBehaviour
{
    public Image[] tabIcons; // Array of tab icons
    public GameObject[] pages; // Array of pages corresponding to each tab


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
                tabIcons[i].color = Color.white; // Active tab icon color
                pages[i].SetActive(true); // Show corresponding page
            }
            else
            {
                tabIcons[i].color = Color.gray; // Inactive tab icon color
                pages[i].SetActive(false); // Hide other pages
            }
        }
    }
}