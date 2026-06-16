// This controls the ACTUAL main menu 
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panel References")]
    public GameObject mainMenuPanel;
    public GameObject hubWorldRoot; 
    public GameObject npcPromptText;

    [Header("Transition Settings")]
    public Image fadeOverlay;
    public float fadeDuration = 0.5f;

    void Awake()
    {    
            // If returning from battle, skip the main menu entirely
        if (SceneHistory.Instance != null && SceneHistory.Instance.hasStoredPosition)
        {
            if (mainMenuPanel != null)
                mainMenuPanel.SetActive(false);

            if (hubWorldRoot != null)
                hubWorldRoot.SetActive(true);

            if (npcPromptText != null)
                npcPromptText.SetActive(true);

            if (fadeOverlay != null)
                fadeOverlay.color = new Color(0, 0, 0, 0);

            return; // skip the rest of Awake
        }

        // Makes sure the main menu is shown
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);

        // Keep hub world hidden until Play is pressed
        if (hubWorldRoot != null)
            hubWorldRoot.SetActive(false);

        if (npcPromptText != null)
            npcPromptText.SetActive(false);

        if (fadeOverlay != null)
            fadeOverlay.color = new Color(0, 0, 0, 0);
    }

    // Play button transitions to the hub world 
    public void OnPlayButton()
    {
        StartCoroutine(FadeToHub());
    }

    public void OnExitButton()
    {
        StartCoroutine(FadeThenQuit());
    }

    IEnumerator FadeToHub()
    {
        if (fadeOverlay != null)
            yield return StartCoroutine(Fade(1f));

        // Swap panels and hides the main menu 
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false);
        
        // shows the hub world
        if (hubWorldRoot != null)
            hubWorldRoot.SetActive(true);

        if (npcPromptText != null)
            npcPromptText.SetActive(true);

        // Fade back in over the hub world
        if (fadeOverlay != null)
            yield return StartCoroutine(Fade(0f));
    }

    IEnumerator FadeThenQuit()
    {
        if (fadeOverlay != null)
            yield return StartCoroutine(Fade(1f));

        Debug.Log("Quitting game...");
        Application.Quit();
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
}