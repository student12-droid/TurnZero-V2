using UnityEngine;

public class BallPhysics : MonoBehaviour
{
    private Rigidbody2D rb;
    public float launchSpeed = 2f;
    public float maxSpeed;

    [Header("Speed Decay")]
    public float decayRate = 0.98f; // Multiplied each FixedUpdate 
    public float minSpeed = 1f;     // Ball never drops below this speed

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        maxSpeed = launchSpeed * 3f;
        rb.linearVelocity = new Vector2(1, -1).normalized * launchSpeed;
    }

    void FixedUpdate()
    {
        float currentSpeed = rb.linearVelocity.magnitude;

        if (currentSpeed > minSpeed)
        {
            // Gradually bleed off speed each physics tick
            float newSpeed = Mathf.Max(currentSpeed * decayRate, minSpeed);
            rb.linearVelocity = rb.linearVelocity.normalized * newSpeed;
        }
    }
}