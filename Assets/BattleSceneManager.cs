// Manages the battle scene UI and transitions
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class BattleSceneManager : MonoBehaviour
{
    [Header("Panel References")]
    public GameObject battlePanel;

    [Header("Transition Settings")]
    public Image fadeOverlay;
    public float fadeDuration = 0.5f;
    public string hubSceneName = "TurnZero_Main Game";

    void Awake()
    {
        if (battlePanel != null)
            battlePanel.SetActive(true);

        if (fadeOverlay != null)
            fadeOverlay.color = new Color(0, 0, 0, 0);
    }

    // Call this from your NPC dialogue trigger to start the battle UI
    public void OnBattleStart()
    {
        StartCoroutine(FadeInBattlePanel());
    }

    // Called when the player flees the battle
    public void OnFleeBattle()
    {
        StartCoroutine(FadeToHub());
    }

    IEnumerator FadeInBattlePanel()
    {
        if (fadeOverlay != null)
            yield return StartCoroutine(Fade(1f));

        if (battlePanel != null)
            battlePanel.SetActive(true);

        if (fadeOverlay != null)
            yield return StartCoroutine(Fade(0f));
    }

    IEnumerator FadeToHub()
    {
        if (fadeOverlay != null)
            yield return StartCoroutine(Fade(1f));

        Debug.Log("Returning to Hub: " + hubSceneName);
        SceneManager.LoadScene(hubSceneName);
    }

    IEnumerator Fade(float targetAlpha)
    {
        if (fadeOverlay == null) yield break;

        float elapsed = 0f;
        float startAlpha = fadeOverlay.color.a;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;
            fadeOverlay.color = new Color(0, 0, 0, Mathf.Lerp(startAlpha, targetAlpha, t));
            yield return null;
        }

        fadeOverlay.color = new Color(0, 0, 0, targetAlpha);
    }
}