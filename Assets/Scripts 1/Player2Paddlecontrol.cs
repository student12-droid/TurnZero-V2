using UnityEngine;

public class Player2Paddlecontrol : MonoBehaviour
{
    private Rigidbody2D rb;
    public float moveSpeed = 15f;
    private Vector2 movementInput;

    [Header("Table Boundaries (Top Half)")]
    public float minX = -3.6f;
    public float maxX = 3.4f;

    [Tooltip("The exact physical center of your neon table — paddle cannot go below this!")]
    public float centerLineY = 1.3203f;

    public float maxY = 8.08f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Spawn dead center horizontally, pinned to the top wall
        Vector2 startPos = new Vector2(0f, maxY);
        transform.position = startPos;
        rb.position = startPos;
    }

    void Update()
    {
        // FREEZE P2 IF IT IS P1'S TURN
        if (TurnManager.Instance.isPlayer1Turn)
        {
            movementInput = Vector2.zero; // Force them to drop the controls
            if (rb != null) rb.linearVelocity = Vector2.zero; // Kill drifting momentum
            return; // Abort reading input!
        }

        // Only read these keys if it IS their turn!
        movementInput = Vector2.zero;

        if (Input.GetKey(KeyCode.UpArrow)) movementInput.y = 1;
        if (Input.GetKey(KeyCode.DownArrow)) movementInput.y = -1;
        if (Input.GetKey(KeyCode.LeftArrow)) movementInput.x = -1;
        if (Input.GetKey(KeyCode.RightArrow)) movementInput.x = 1;
    }

    void FixedUpdate()
    {
        // Stop physical movement if frozen
        if (TurnManager.Instance.isPlayer1Turn) return;

        Vector2 newPosition = rb.position + movementInput.normalized * moveSpeed * Time.fixedDeltaTime;

        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, centerLineY, maxY);

        rb.MovePosition(newPosition);
    }
}