using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelFader : MonoBehaviour
{
    [Header("Transition Settings")]
    [Tooltip("Drag your black curtain Image here")]
    public Image fadeCurtain;
    public float fadeSpeed = 1.5f;

    void Start()
    {
        //Check for the black curtain and set it to fully opaque at the start of the level
        if (fadeCurtain != null)
        {
            fadeCurtain.color = new Color(0, 0, 0, 1f);
            
            // 2. ...and immediately fade to clear to reveal the level!
            StartCoroutine(FadeIn());
        }
    }

    // Call this from an NPC, a Button, or anywhere else
    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeOutAndLoad(sceneName));
    }

    private IEnumerator FadeIn()
    {
        fadeCurtain.raycastTarget = true; // Block clicks while fading

        float alpha = 1f;
        while (alpha > 0f)
        {
            alpha -= Time.deltaTime * fadeSpeed;
            fadeCurtain.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        fadeCurtain.raycastTarget = false; // Allow clicks again!
    }

    private IEnumerator FadeOutAndLoad(string sceneName)
    {
        fadeCurtain.raycastTarget = true;

        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha -= -Time.deltaTime * fadeSpeed;
            fadeCurtain.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        // Once the screen is black, load the new scene
        SceneManager.LoadScene(sceneName);
    }
}