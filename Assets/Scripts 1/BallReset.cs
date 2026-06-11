using UnityEngine;

public class BallReset : MonoBehaviour
{
    private Vector2 startPosition;
    private Rigidbody2D rb;

    void Start()
    {
        // The starting position of the ball
        rb = GetComponent<Rigidbody2D>();
    }

    // We will call this command from the TurnManager
    public void ResetPosition()
    {
        // 1. Teleport back to the center
        transform.position = startPosition;

        // 2. Kill all physical momentum so it doesn't keep sliding
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }
}