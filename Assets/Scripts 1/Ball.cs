using UnityEngine;
using TurnZero.Minigame;

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
        // Process score zone collisions.
        if (col.gameObject.CompareTag("TargetP1")) ScoreManager.Instance.AddP1Hit();
        else if (col.gameObject.CompareTag("TargetP2")) ScoreManager.Instance.AddP2Hit();

        // Detect collision with a defense wall component.
        DefenseWall wall = col.gameObject.GetComponent<DefenseWall>();
        if (wall != null)
        {
            // Evaluate active power-up states.
            bool isPoweredUp = isFireball || isIceball;
            
            // Transmit damage signal to the wall.
            wall.TakeHit(isPoweredUp);

            // Strip power-up properties post-impact to restore standard physics.
            if (isPoweredUp) 
            {
                StripPowerUp();
            }
        }
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

    // Reverts the ball to its default visual and logical state.
    private void StripPowerUp()
    {
        GetComponent<SpriteRenderer>().color = Color.green;
        isFireball = false; 
        isIceball = false; 
    }

    public void ResetBall()
    {
        // Center the ball position and arrest all current velocity.
        transform.position = Vector2.zero;
        rb.linearVelocity = Vector2.zero;
        
        // Clear active power-ups prior to the new serve.
        StripPowerUp(); 

        Vector2 launchDirection = new Vector2(1f, 1f).normalized;

        // Calculate serve trajectory based on active turn state.
        if (TurnManager.Instance != null)
        {
            if (TurnManager.Instance.IsPlayer1Turn) launchDirection = new Vector2(1f, 1f).normalized; 
            else launchDirection = new Vector2(-1f, -1f).normalized; 
        }

        LaunchBall(launchDirection);
    }

    private void LaunchBall(Vector2 direction)
    {
        rb.linearVelocity = direction * speed;
    }
}