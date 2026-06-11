//Controls the main menu stuff
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Panel References")]
    public GameObject mainMenuPanel;
    public GameObject battlePanel;
    public GameObject storyModePanel;

    [Header("Transition Settings")]
    public Image fadeOverlay; 
    public float fadeDuration = 0.5f;
    public string hubSceneName = "TurnZero_Main Game";

    void Awake()
    {
        // Ensures the game starts on the menu
        if (mainMenuPanel != null && battlePanel != null)
        {
            mainMenuPanel.SetActive(true);  
            battlePanel.SetActive(false); 
            storyModePanel.SetActive(false);  
        }
        
        //Ensures transition overlay is hidden
        if (fadeOverlay != null)
            fadeOverlay.color = new Color(0, 0, 0, 0);
    }

    public void OnPlayButton()
    {
        StartCoroutine(FadeTransition("battle"));
    }

     public void OnCareerButton() 
    {
        StartCoroutine(FadeTransition("story"));
    }
    public void OnBackToMenu()
    {
        StartCoroutine(FadeTransition("menu"));
    }

    IEnumerator FadeToHub()
    {
        if (fadeOverlay != null)
        {
            //Fades to Black
            yield return StartCoroutine(Fade(1f));
        }

        Debug.Log("Returning to Hub: " + hubSceneName);

        SceneManager.LoadScene(hubSceneName);
    }

    IEnumerator Fade(float targetAlpha)
    {
        float elapsed = 0f;
        float startAlpha = fadeOverlay.color.a;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / fadeDuration);
            fadeOverlay.color = new Color(0, 0, 0, newAlpha);
            yield return null;
        }
        fadeOverlay.color = new Color(0, 0, 0, targetAlpha);
    }
    IEnumerator FadeTransition(string destination)
    {
        if (fadeOverlay == null)
        {
            // Fallback if you forgot to assign the overlay
            SwapPanels(destination);
            yield break;
        }

        // Fade the screen to black
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            fadeOverlay.color = new Color(0, 0, 0, elapsed / fadeDuration);
            yield return null;
        }

        // Swap Panels while screen is black
        SwapPanels(destination);

        // Fade back to Transparent
        elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            fadeOverlay.color = new Color(0, 0, 0, 1 - (elapsed / fadeDuration));
            yield return null;
        }
        
        fadeOverlay.color = new Color(0, 0, 0, 0);
    }

    void SwapPanels(string destination)
    {
        mainMenuPanel.SetActive(false);
        battlePanel.SetActive(false);
        storyModePanel.SetActive(false);
        
        //shows the correct panel
        switch (destination)
        {
            case "battle":
                battlePanel.SetActive(true);
                break;
            case "story":
                storyModePanel.SetActive(true);
                break;
            case "menu":
                mainMenuPanel.SetActive(true);
                break;
        }
        Debug.Log("Opened " + destination);
    }
    
    public void OnQuitButton()
    {
        StartCoroutine(FadeToHub()); 
    }
}
