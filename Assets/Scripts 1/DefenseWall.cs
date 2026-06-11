using UnityEngine;

public class DefenseWall : MonoBehaviour
{
    [Header("Wall Settings")]
    public int maxHits = 15;
    private int currentHits = 0;

    [Header("Linked Fireball Call")]
    public GameObject linkedFireballCall;

    private Renderer wallRenderer;

    void Start()
    {
        wallRenderer = GetComponent<Renderer>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject ball = collision.gameObject;

        if (ball.CompareTag("Fireball") || ball.CompareTag("Iceball"))
        {
            TakeHit(5);
        }
        else if (ball.CompareTag("Player"))
        {
            TakeHit(1);
        }
    }

    void TakeHit(int amount)
    {
        currentHits += amount;
        Debug.Log($"Wall hit! Total hits: {currentHits}/{maxHits}");

        if (currentHits >= maxHits)
        {
            DestroyWall();
        }
    }

    void DestroyWall()
    {
        Debug.Log("Wall destroyed!");

        if (wallRenderer != null)
            wallRenderer.enabled = false;

        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        // Disable the entire linked Fireball Call GameObject
        if (linkedFireballCall != null)
        {
            linkedFireballCall.SetActive(false);
            Debug.Log($"Disabled linked fireball call: {linkedFireballCall.name}");
        }
    }

    public void ResetWall()
    {
        currentHits = 0;

        if (wallRenderer != null)
            wallRenderer.enabled = true;

        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = true;

        if (linkedFireballCall != null)
            linkedFireballCall.SetActive(true);

        Debug.Log("Wall reset!");
    }

    public float GetHitPercentage()
    {
        return (float)currentHits / maxHits;
    }
}