using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI loadingText;
    public RectTransform spinner;

    [Header("Settings")]
    public float spinSpeed = 200f;
    public float loadingDuration = 3f;
    public string nextSceneName = "SampleScene";

    private string baseText = "Loading";
    private float dotTimer = 0f;
    private float dotInterval = 0.4f;
    private int dotCount = 0;

    void Start()
    {
        StartCoroutine(LoadAfterDelay());
    }

    void Update()
    {
        spinner.Rotate(0f, 0f, -spinSpeed * Time.deltaTime);
        dotTimer += Time.deltaTime;
        if (dotTimer >= dotInterval)
        {
            dotTimer = 0f;
            dotCount = (dotCount + 1) % 4;
            loadingText.text = baseText + new string('.', dotCount);
        }
    }

    IEnumerator LoadAfterDelay()
    {
        yield return new WaitForSeconds(loadingDuration);
        SceneManager.LoadScene(nextSceneName);
    }
}