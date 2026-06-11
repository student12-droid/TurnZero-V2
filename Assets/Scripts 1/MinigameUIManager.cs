using UnityEngine;

public class MinigameUIManager : MonoBehaviour
{
    [Header("Menu Panels")]
    [Tooltip("Drag the GameObject holding Career/QuickPlay/Quit here")]
    public GameObject mainMenuPanel; 
    
    [Tooltip("Drag your StoryModePanel here")]
    public GameObject storyModePanel;

    void Start()
    {
        // Check if the player came directly from the 3D Career NPC
        if (TransitionManager.skipToCareer)
        {
            Debug.Log("Signal received! Bypassing Main Menu for Career Mode.");
            
            // Turn off the main menu, turn on Career mode
            if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
            if (storyModePanel != null) storyModePanel.SetActive(true);

            // Reset the flare 
            TransitionManager.skipToCareer = false;
        }
        else
        {
            Debug.Log("Normal load. Showing Main Menu.");
            
            // Default behavior if  Play is hit
            if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
            if (storyModePanel != null) storyModePanel.SetActive(false);
        }
    }
}