using UnityEngine;

public class LocalCoopPaddle : MonoBehaviour
{
    private Rigidbody2D rb;
    public float moveSpeed = 15f;
    private Vector2 movementInput;

    [Header("Player Setup")]
    [Tooltip("Check this if this is the Bottom paddle (WASD). Uncheck for Top paddle (Arrows).")]
    public bool isPlayer1 = true;

    [Header("Table Boundaries")]
    public float minX = -8f;
    public float maxX = 8f;
    public float minY = -4f;
    public float maxY = 4f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movementInput = Vector2.zero;

        // PLAYER 1: Bottom Paddle (WASD)
        if (isPlayer1)
        {
            if (Input.GetKey(KeyCode.W)) movementInput.y = 1;
            if (Input.GetKey(KeyCode.S)) movementInput.y = -1;
            if (Input.GetKey(KeyCode.A)) movementInput.x = -1;
            if (Input.GetKey(KeyCode.D)) movementInput.x = 1;
        }
        // PLAYER 2: Top Paddle (Arrow Keys)
        else
        {
            if (Input.GetKey(KeyCode.UpArrow)) movementInput.y = 1;
            if (Input.GetKey(KeyCode.DownArrow)) movementInput.y = -1;
            if (Input.GetKey(KeyCode.LeftArrow)) movementInput.x = -1;
            if (Input.GetKey(KeyCode.RightArrow)) movementInput.x = 1;
        }
    }

    void FixedUpdate()
    {
        Vector2 newPosition = rb.position + movementInput.normalized * moveSpeed * Time.fixedDeltaTime;

        // X-Axis: Both players can move across the whole width
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);

        // Y-Axis (The Center Line Rule)
        if (isPlayer1)
        {
            // P1 is trapped on the bottom half
            newPosition.y = Mathf.Clamp(newPosition.y, minY, -0.5f);
        }
        else
        {
            // P2 is trapped on the top half
            newPosition.y = Mathf.Clamp(newPosition.y, 0.5f, maxY);
        }

        rb.MovePosition(newPosition);
    }
}