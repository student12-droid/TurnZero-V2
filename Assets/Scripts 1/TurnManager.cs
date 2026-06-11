using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;

    [Header("Turn State")]
    public bool isPlayer1Turn = true;

    [Header("Player 1's Turn Items")]
    public GameObject p1DefenceWall;
    public GameObject p1FireballTarget;

    [Header("Player 2's Turn Items")]
    public GameObject p2DefenceWall;
    public GameObject p2FireballTarget;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Start the game by defaulting to Player 1's turn
        SetPlayer1Turn();
    }

    public void SwitchTurn()
    {
        if (isPlayer1Turn) SetPlayer2Turn();
        else SetPlayer1Turn();

        // Teleport the ball to center upon turn swap
        Ball gameBall = FindFirstObjectByType<Ball>();
        if (gameBall != null) gameBall.ResetBall();
    }

    public void SetPlayer1Turn()
    {
        isPlayer1Turn = true;

        // P1 active, P2 inactive
        if (p1DefenceWall != null) p1DefenceWall.SetActive(true);
        if (p1FireballTarget != null) p1FireballTarget.SetActive(true);
        
        if (p2DefenceWall != null) p2DefenceWall.SetActive(false);
        if (p2FireballTarget != null) p2FireballTarget.SetActive(false);
        
        Debug.Log("Turn Swapped: Player 1's Turn");
    }

    public void SetPlayer2Turn()
    {
        isPlayer1Turn = false;

        // P2 active, P1 inactive
        if (p1DefenceWall != null) p1DefenceWall.SetActive(false);
        if (p1FireballTarget != null) p1FireballTarget.SetActive(false);
        
        if (p2DefenceWall != null) p2DefenceWall.SetActive(true);
        if (p2FireballTarget != null) p2FireballTarget.SetActive(true);

        Debug.Log("Turn Swapped: Player 2's Turn");
    }
    void FixedUpdate()
{
    // If it is NOT this player's turn, kill all movement and exit!
    if (this.gameObject.name == "Player1" && !TurnManager.Instance.isPlayer1Turn) return;
    if (this.gameObject.name == "Player2" && TurnManager.Instance.isPlayer1Turn) return;

    // ... your normal movement code below ...
}
}