using UnityEngine;

public class Player1Paddlecontrol : MonoBehaviour
{
    private Rigidbody2D rb;
    public float moveSpeed = 15f;
    private Vector2 movementInput;

    [Header("Table Boundaries (Bottom Half)")]
    public float minX = -3.6f;
    public float maxX = 3.4f;
    public float minY = -5.28f;

    [Tooltip("The exact physical center of your neon table — paddle cannot go above this!")]
    public float centerLineY = 1.3203f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Spawn dead center horizontally, pinned to the bottom wall
        Vector2 startPos = new Vector2(0f, minY);
        transform.position = startPos;
        rb.position = startPos;
    }

    void Update()
    {
        // FREEZE P1 IF IT IS NOT P1'S TURN
        if (!TurnManager.Instance.isPlayer1Turn)
        {
            movementInput = Vector2.zero; // Force them to drop the controls
            if (rb != null) rb.linearVelocity = Vector2.zero; // Kill drifting momentum
            return; // Abort reading input!
        }

        // Only read these keys if it IS their turn!
        movementInput = Vector2.zero;
        if (Input.GetKey(KeyCode.W)) movementInput.y = 1;
        if (Input.GetKey(KeyCode.S)) movementInput.y = -1;
        if (Input.GetKey(KeyCode.A)) movementInput.x = -1;
        if (Input.GetKey(KeyCode.D)) movementInput.x = 1;
    }

    void FixedUpdate()
    {
        // Stop physical movement if frozen
        if (!TurnManager.Instance.isPlayer1Turn) return;

        Vector2 newPosition = rb.position + movementInput.normalized * moveSpeed * Time.fixedDeltaTime;

        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, centerLineY);

        rb.MovePosition(newPosition);
        
    }   
}