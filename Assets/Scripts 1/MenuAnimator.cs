// Deals with animating the menu elements
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class MenuAnimator : MonoBehaviour
{
    [Header("Background Elements")]
    public CanvasGroup gridCanvasGroup;
    public CanvasGroup bigZeroCanvasGroup;

    [Header("Main Elements")]
    public RectTransform titleRect;
    public CanvasGroup titleCanvas;
    public RectTransform playButton;
    public CanvasGroup playCanvas;
    public RectTransform quitButton;
    public CanvasGroup quitCanvas;
    public RectTransform storyButton; 
    public CanvasGroup storyCanvas;   

    [Header("Decorations")]
    public RectTransform bigDiamond;
    public RectTransform[] smallDiamonds;
    public RectTransform[] pillars;

    [Header("Timing")]
    public float introDuration = 0.7f;
    
    //Stores their initial anchored postion
    private Vector2 bigDiamondOrigin;
    private Vector2[] smallDiamondOrigins;
    private Vector2[] pillarOrigins;
    private Vector2 titleOrigin;
    private Vector2 playOrigin;
    private Vector2 quitOrigin;
    private Vector2 storyOrigin;

    void Start()
    {
        //Their positions after the intro animation
        titleOrigin = titleRect.anchoredPosition;
        playOrigin = playButton.anchoredPosition;
        quitOrigin = quitButton.anchoredPosition;
        if (storyButton != null) storyOrigin = storyButton.anchoredPosition;

        if (bigDiamond) bigDiamondOrigin = bigDiamond.anchoredPosition;

        if (smallDiamonds != null)
        {
            smallDiamondOrigins = new Vector2[smallDiamonds.Length];
            for (int i = 0; i < smallDiamonds.Length; i++)
                smallDiamondOrigins[i] = smallDiamonds[i].anchoredPosition;
        }

        if (pillars != null)
        {
            pillarOrigins = new Vector2[pillars.Length];
            for (int i = 0; i < pillars.Length; i++)
                pillarOrigins[i] = pillars[i].anchoredPosition;
        }

        StartCoroutine(PlayIntro());
    }

    void Update()
    {
        //Handles constant animations that play after the intro

        //Animates some diamonds to float up and down and rotate
        if (bigDiamond)
        {
            float floatY = Mathf.Sin(Time.time * 1.2f) * 10f;
            float floatRot = Mathf.Sin(Time.time * 0.8f) * 8f;
            bigDiamond.anchoredPosition = bigDiamondOrigin + new Vector2(0, floatY);
            bigDiamond.localRotation = Quaternion.Euler(0, 0, 45f + floatRot);
        }

        //Animates other diamonds to float and rotate independently
        if (smallDiamonds != null)
        {
            for (int i = 0; i < smallDiamonds.Length; i++)
            {
                float offset = i * 1.3f;
                float floatY = Mathf.Sin((Time.time + offset) * 1.5f) * 6f;
                float floatX = Mathf.Cos((Time.time + offset) * 0.9f) * 4f;
                smallDiamonds[i].anchoredPosition = smallDiamondOrigins[i] + new Vector2(floatX, floatY);
                smallDiamonds[i].localRotation = Quaternion.Euler(0, 0, 45f + Mathf.Sin(Time.time + offset) * 15f);
            }
        }
    }

    IEnumerator PlayIntro()
    {   
        // decides their position and alpha when their offscreen states
        titleCanvas.alpha = 0f;
        playCanvas.alpha = 0f;
        quitCanvas.alpha = 0f;
        if (storyCanvas != null) storyCanvas.alpha = 0f;
        titleRect.anchoredPosition = titleOrigin + new Vector2(0, -80f);
        playButton.anchoredPosition = playOrigin + new Vector2(0, -50f);
        quitButton.anchoredPosition = quitOrigin + new Vector2(0, -50f);
        if (storyButton != null) storyButton.anchoredPosition = storyOrigin + new Vector2(0, -50f);

        //initiates the pillar animation to pull them onscreen
        if (pillars != null)
        {
            for (int i = 0; i < pillars.Length; i++)
            {
                float direction = (i % 2 == 0) ? -1f : 1f;
                pillars[i].anchoredPosition = pillarOrigins[i] + new Vector2(direction * 300f, 0);
            }
            StartCoroutine(AnimatePillars());
        }

        if (bigDiamond)
        {
            bigDiamond.localScale = Vector3.zero;
            StartCoroutine(ScaleIn(bigDiamond, 0.3f, 0.5f));
        }

        if (smallDiamonds != null)
        {
            for (int i = 0; i < smallDiamonds.Length; i++)
            {
                smallDiamonds[i].localScale = Vector3.zero;
                float delay = 0.4f + i * 0.1f;
                StartCoroutine(ScaleIn(smallDiamonds[i], delay, 0.35f));
            }
        }

    //Introduce the UI elements with slight delays
    yield return new WaitForSeconds(0.25f);
        StartCoroutine(FadeAndRise(titleRect, titleCanvas, titleOrigin, introDuration));

        yield return new WaitForSeconds(0.35f);
        StartCoroutine(FadeAndRise(playButton, playCanvas, playOrigin, introDuration * 0.85f));

        yield return new WaitForSeconds(0.12f);
        StartCoroutine(FadeAndRise(quitButton, quitCanvas, quitOrigin, introDuration * 0.85f));

       
        if (storyButton != null && storyCanvas != null)
        {
            yield return new WaitForSeconds(0.12f); 
            StartCoroutine(FadeAndRise(storyButton, storyCanvas, storyOrigin, introDuration * 0.85f));
        }
        //fades in specifically the grid and the zero
        StartCoroutine(FadeInOnly(gridCanvasGroup, 0.5f, 0.1f, introDuration));
        StartCoroutine(FadeInOnly(bigZeroCanvasGroup, 0.13f, 0.2f, introDuration));

    }


    IEnumerator AnimatePillars()
    {
        float elapsed = 0f;
        float duration = 0.8f;
        Vector2[] startPositions = new Vector2[pillars.Length];
        for (int i = 0; i < pillars.Length; i++)
            startPositions[i] = pillars[i].anchoredPosition;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsed / duration);
            for (int i = 0; i < pillars.Length; i++)
                pillars[i].anchoredPosition = Vector2.Lerp(startPositions[i], pillarOrigins[i], t);
            yield return null;
        }
    }

    IEnumerator FadeAndRise(RectTransform rt, CanvasGroup cg, Vector2 targetPos, float duration)
    {
        Vector2 startPos = rt.anchoredPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsed / duration);
            rt.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
            cg.alpha = Mathf.Lerp(0, 1, t);
            yield return null;
        }

        rt.anchoredPosition = targetPos;
        cg.alpha = 1f;
    }

    IEnumerator ScaleIn(RectTransform rt, float delay, float duration)
    {
        yield return new WaitForSeconds(delay);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsed / duration);
            float scale = t < 0.8f
                ? Mathf.Lerp(0, 1.2f, t / 0.8f)
                : Mathf.Lerp(1.2f, 1f, (t - 0.8f) / 0.2f);
            rt.localScale = new Vector3(scale, scale, 1f);
            yield return null;
        }

        rt.localScale = Vector3.one;
    }
    
    //For the fade effect when transitioning panels
    IEnumerator FadeInOnly(CanvasGroup cg, float targetAlpha, float delay, float duration)
    {
        if (cg == null) yield break;
        cg.alpha = 0f;
        yield return new WaitForSeconds(delay);
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            cg.alpha = Mathf.Lerp(0, targetAlpha, elapsed / duration);
            yield return null;
        }
        cg.alpha = targetAlpha;
    }

}