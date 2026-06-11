using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody2D rb;
    
    [Header("Movement Settings")]
    public float speed = 5f; 

    [Header("State Memory")]
    public bool isFireball = false; 
    public bool isIceball = false; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ResetBall();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("TargetP1")) ScoreManager.Instance.AddP1Hit();
        else if (col.gameObject.CompareTag("TargetP2")) ScoreManager.Instance.AddP2Hit();
    }

    public void ActivateFireball()
    {
        Debug.Log("Fireball attack activated!");
        GetComponent<SpriteRenderer>().color = Color.red;
        isFireball = true; 
        isIceball = false;
    }

    public void ActivateIceball()
    {
        Debug.Log("Iceball attack activated!");
        GetComponent<SpriteRenderer>().color = Color.cyan; 
        isIceball = true;
        isFireball = false;
    }

    public void ResetBall()
    {
        // Snap instantly back to the dead center of the arena
        transform.position = Vector2.zero;
        
        // Kill all leftover momentum from the previous rally
        rb.linearVelocity = Vector2.zero;
        
        // Reset visual state and special attacks
        GetComponent<SpriteRenderer>().color = Color.green;
        isFireball = false; 
        isIceball = false; 

        Vector2 launchDirection = new Vector2(1f, 1f).normalized;

        // Check the Turn Manager to figure out which way to serve the ball
        if (TurnManager.Instance != null)
        {
            if (TurnManager.Instance.isPlayer1Turn)
            {
                // P1's Turn: Shoot towards the right
                launchDirection = new Vector2(1f, 1f).normalized; 
            }
            else
            {
                // P2's Turn: Shoot towards the left
                launchDirection = new Vector2(-1f, -1f).normalized; 
            }
        }

        LaunchBall(launchDirection);
    }

    private void LaunchBall(Vector2 direction)
    {
        rb.linearVelocity = direction * speed;
    }
}