using UnityEngine;
using TMPro;

public class DefenseWall : MonoBehaviour
{
    [Header("Wall Settings")]
    public int maxHits = 15;
    private int currentHits = 0;

    [Header("UI Display")]
    public TMP_Text wallHealthText; 
    public string wallOwnerName; 

    [Header("Linked Fireball Call")]
    public GameObject linkedFireballCall;

    private Renderer wallRenderer;
    private Collider2D wallCollider;

    void Start()
    {
        wallRenderer = GetComponent<Renderer>();
        wallCollider = GetComponent<Collider2D>();

        // We remove the LevelManager direct read from here to prevent execution timing bugs
        UpdateHealthUI();
    }

    // Processes damage taken by the wall. Intended to be invoked externally by the Ball.
    public void TakeHit(bool isPowerUp)
    {
        if (isPowerUp)
        {
            currentHits = 5;
            Debug.Log("POWER-UP IMPACT! Wall health crippled to 10 hits remaining!");
        }
        else
        {
            currentHits++;
            Debug.Log($"Normal impact. Total hits: {currentHits}/{maxHits}");
        }

        UpdateHealthUI();

        if (wallRenderer != null)
        {
            float healthRemaining = 1f - ((float)currentHits / maxHits);
            wallRenderer.material.color = Color.Lerp(Color.red, Color.white, healthRemaining);
        }

        if (currentHits >= maxHits)
        {
            DestroyWall();
        }
    }

    // Forces a refresh of the TextMeshPro display component numbers.
    public void UpdateHealthUI()
    {
        if (wallHealthText != null)
        {
            int hitsRemaining = maxHits - currentHits;
            if (hitsRemaining < 0) hitsRemaining = 0; 

            wallHealthText.text = $"{wallOwnerName}: {hitsRemaining} HITS";
        }
    }

    // Returns true if the wall has reached or exceeded its maximum hit threshold.
    public bool IsDestroyed()
    {
        return currentHits >= maxHits;
    }

    void DestroyWall()
    {
        Debug.Log("Wall destroyed!");

        if (wallRenderer != null) wallRenderer.enabled = false;
        if (wallCollider != null) wallCollider.enabled = false;

        if (wallHealthText != null)
        {
            wallHealthText.text = $"{wallOwnerName}: DESTROYED";
        }

        if (linkedFireballCall != null)
        {
            linkedFireballCall.SetActive(false);
        }
    }

    public void ResetWall()
    {
        currentHits = 0;
        UpdateHealthUI();

        if (wallRenderer != null)
        {
            wallRenderer.enabled = true;
            wallRenderer.material.color = Color.white; 
        }
        
        if (wallCollider != null) wallCollider.enabled = true;
        if (linkedFireballCall != null) linkedFireballCall.SetActive(true);

        Debug.Log("Wall reset!");
    }
}