using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class StoryManager : MonoBehaviour
{
    [Header("Settings")]
    // The actual scene we want to load. No sneaky spaces allowed here.
    public string nextSceneName = "TurnZero_Main Game";

    [Header("Canvas Groups")]
    [SerializeField] private CanvasGroup titleGroup;
    [SerializeField] private CanvasGroup usernameGroup;
    [SerializeField] private CanvasGroup dialogueGroup;
    [SerializeField] private CanvasGroup choiceGroup;
    [SerializeField] private CanvasGroup narrationGroup;
    [SerializeField] private CanvasGroup fogGroup;
    [SerializeField] private CanvasGroup fireballGroup;
    [SerializeField] private CanvasGroup flashGroup;

    [Header("Text Elements")]
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI speakerText;
    [SerializeField] private TextMeshProUGUI narrationText;
    [SerializeField] private TextMeshProUGUI choiceAText;
    [SerializeField] private TextMeshProUGUI choiceBText;

    [Header("UI Controls")]
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private Button startButton;

    [Header("Other Elements")]
    [SerializeField] private RectTransform fireballObject;
    [SerializeField] private AudioSource themeMusic;
    [SerializeField] private Image characterImage;

    [Header("Background")]
    [SerializeField] private Image backgroundImage;
    public Sprite[] backgrounds;

    private string playerName = "Traveller";
    private bool storyStarted = false;

    void Start()
    {
        // Grab the music component if we forgot to link it in the inspector
        if (themeMusic == null) themeMusic = GetComponent<AudioSource>();

        // Make sure these start totally transparent so they don't pop in weirdly
        if (backgroundImage != null)
            backgroundImage.color = new Color(1, 1, 1, 0);

        if (characterImage != null)
            characterImage.color = new Color(1, 1, 1, 0);

        HideAll();
        StartCoroutine(ShowTitle());

        // Hook up the start button
        if (startButton != null) startButton.onClick.AddListener(OnStartPressed);
    }

    void HideAll()
    {
        SetGroup(titleGroup, 0);
        SetGroup(usernameGroup, 0);
        SetGroup(dialogueGroup, 0);
        SetGroup(choiceGroup, 0);
        SetGroup(narrationGroup, 0);
        SetGroup(fogGroup, 0);
        SetGroup(fireballGroup, 0);
        SetGroup(flashGroup, 0);
    }

    void SetGroup(CanvasGroup g, float a)
    {
        if (g == null) return;
        g.alpha = a;
        
        // Don't let players click invisible buttons
        g.interactable = a > 0;
        g.blocksRaycasts = a > 0;
    }

    IEnumerator Fade(CanvasGroup g, float target, float duration)
    {
        if (g == null) yield break;
        float start = g.alpha;
        float t = 0;

        if (duration <= 0)
        {
            g.alpha = target;
        }
        else
        {
            // Smoothly transition the alpha over time
            while (t < duration)
            {
                t += Time.deltaTime;
                g.alpha = Mathf.Lerp(start, target, t / duration);
                yield return null;
            }
            g.alpha = target;
        }

        g.interactable = target > 0;
        g.blocksRaycasts = target > 0;
    }

    void SetBackground(int index, float alpha)
    {
        if (backgroundImage == null) return;

        // If the index is out of bounds, just go to a black screen
        if (index < 0 || index >= backgrounds.Length)
        {
            backgroundImage.sprite = null;
            backgroundImage.color = new Color(0, 0, 0, 1);
        }
        else
        {
            backgroundImage.sprite = backgrounds[index];
            backgroundImage.color = new Color(1, 1, 1, alpha);
        }
    }

    IEnumerator FadeBackground(float targetAlpha, float duration)
    {
        if (backgroundImage == null) yield break;
        float startAlpha = backgroundImage.color.a;
        float t = 0;
        
        while (t < duration)
        {
            t += Time.deltaTime;
            Color c = backgroundImage.color;
            backgroundImage.color = new Color(c.r, c.g, c.b, Mathf.Lerp(startAlpha, targetAlpha, t / duration));
            yield return null;
        }
    }

    IEnumerator FadeCharacter(float targetAlpha, float duration)
    {
        if (characterImage == null) yield break;
        float startAlpha = characterImage.color.a;
        float t = 0;
        
        while (t < duration)
        {
            t += Time.deltaTime;
            Color c = characterImage.color;
            characterImage.color = new Color(c.r, c.g, c.b, Mathf.Lerp(startAlpha, targetAlpha, t / duration));
            yield return null;
        }
        Color final = characterImage.color;
        characterImage.color = new Color(final.r, final.g, final.b, targetAlpha);
    }

    IEnumerator ShowTitle()
    {
        yield return StartCoroutine(Fade(titleGroup, 1, 2f));
        yield return new WaitForSeconds(3f);
        yield return StartCoroutine(Fade(titleGroup, 0, 1f));

        SetBackground(0, 0.15f);

        yield return StartCoroutine(Fade(usernameGroup, 1, 1f));
    }

    void OnStartPressed()
    {
        // Prevent button mashing from starting the sequence twice
        if (storyStarted) return;
        storyStarted = true;

        if (nameInput != null)
        {
            // Strip out any accidental spaces the player typed in their name
            string n = nameInput.text.Trim();
            if (!string.IsNullOrEmpty(n)) playerName = n;
        }

        StartCoroutine(BeginStory());
    }

    IEnumerator BeginStory()
    {
        yield return StartCoroutine(Fade(usernameGroup, 0, 1f));
        yield return new WaitForSeconds(0.8f);

        SetBackground(0, 0.15f);
        yield return StartCoroutine(ShowNarration("It was supposed to be a normal camping trip..."));
        yield return StartCoroutine(ShowNarration(playerName + " packed a bag, laced their boots, and headed into the woods alone."));

        yield return StartCoroutine(FadeBackground(1f, 1f));
        yield return StartCoroutine(ShowDialogue("", playerName + " gets ready to leave camp."));

        SetBackground(1, 1f);
        yield return StartCoroutine(ShowDialogue("", "A huge wind sweeps through, erasing every trail marker."));
        yield return StartCoroutine(ShowNarration("The trail… where did it go?", Color.black));
        yield return StartCoroutine(Fade(fogGroup, 1, 3f));

        SetBackground(-1, 1f);
        yield return StartCoroutine(ShowNarration("2 Hours Later…"));

        SetBackground(2, 1f);
        yield return StartCoroutine(ShowDialogue(playerName, "I have been walking for hours…"));
        yield return StartCoroutine(ShowDialogue("", "Then… a sound. Something in the bush."));

        if (themeMusic != null) themeMusic.Play();

        SetBackground(3, 1f);
        yield return StartCoroutine(FadeCharacter(1f, 1.5f));
        yield return StartCoroutine(ShowDialogue(playerName, "What is that sound…"));
        yield return StartCoroutine(ShowDialogue(playerName, "Hello…? Who's there…?"));
        yield return StartCoroutine(ShowDialogue(playerName, "What's that…?"));
        yield return StartCoroutine(RiseFireball());
        yield return StartCoroutine(ShowDialogue(playerName, "Wait, is it…?"));
        
        // Time to leave!
        yield return StartCoroutine(Teleport());
    }

    IEnumerator ShowDialogue(string speaker, string line, float typewriterDuration = 3f)
    {
        // Clear out other UI elements so they don't overlap
        SetGroup(narrationGroup, 0);
        SetGroup(choiceGroup, 0);

        if (speakerText != null) speakerText.text = speaker;
        if (dialogueText != null) dialogueText.text = "";

        yield return null;

        yield return StartCoroutine(Fade(dialogueGroup, 1, 0.4f));

        // Classic typewriter text effect
        if (dialogueText != null && line.Length > 0)
        {
            float charDelay = typewriterDuration / line.Length;
            foreach (char c in line)
            {
                dialogueText.text += c;
                yield return new WaitForSeconds(charDelay);
            }
        }

        // Give the player a second to actually read the final text
        yield return new WaitForSeconds(3f);

        yield return StartCoroutine(Fade(dialogueGroup, 0, 0.3f));
    }

    IEnumerator ShowNarration(string line, Color? textColor = null)
    {
        SetGroup(dialogueGroup, 0);
        SetGroup(choiceGroup, 0);

        if (narrationText != null)
        {
            narrationText.text = line;

            Color originalColor = narrationText.color;

            if (textColor.HasValue)
                narrationText.color = textColor.Value;

            yield return StartCoroutine(Fade(narrationGroup, 1, 1f));
            yield return new WaitForSeconds(3f);
            yield return StartCoroutine(Fade(narrationGroup, 0, 1f));

            narrationText.color = originalColor;
        }
        else
        {
            yield return StartCoroutine(Fade(narrationGroup, 1, 1f));
            yield return new WaitForSeconds(3f);
            yield return StartCoroutine(Fade(narrationGroup, 0, 1f));
        }
    }

    IEnumerator RiseFireball()
    {
        SetGroup(dialogueGroup, 0);

        // Snap the fireball to the bottom of the screen before animating it up
        if (fireballObject != null)
        {
            fireballObject.anchoredPosition = new Vector2(0, -600);
        }

        yield return StartCoroutine(Fade(fireballGroup, 1, 1f));

        // Animate the fireball flying upwards
        float t = 0;
        while (t < 2.5f)
        {
            t += Time.deltaTime;
            if (fireballObject != null)
            {
                fireballObject.anchoredPosition = Vector2.Lerp(
                    new Vector2(0, -600),
                    new Vector2(0, 0),
                    t / 2.5f
                );
            }
            yield return null;
        }

        if (fireballObject != null)
        {
            fireballObject.anchoredPosition = new Vector2(0, 0);
        }
    }

    IEnumerator Teleport()
    {
        // Flash the screen white
        yield return StartCoroutine(Fade(flashGroup, 1, 0.5f));
        yield return new WaitForSeconds(0.5f);
        
        // Actually load the main game scene
        SceneManager.LoadScene(0);
    }
}