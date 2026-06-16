using UnityEngine;
using TMPro; 

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance; 

    [Header("Match Score (The Actual Game)")]
    public int p1Score = 0;
    public int p2Score = 0;

    [Header("Defense Streaks (The Central Blocks)")]
    public TMP_Text p1StreakText; 
    public TMP_Text p2StreakText; 
    public int p1Streak = 0;
    public int p2Streak = 0;
    public int requiredHits = 6; 

    [Header("Defense Wall Tracker")]
    private int p1WallHits = 0;
    private int p2WallHits = 0;
    private const int WIN_THRESHOLD = 15;

    [Header("Object Links")]
    public Ball theBall; 

    void Awake() 
    { 
        Instance = this; 
    }

    void Start() 
    { 
        UpdateBothUIs(); 
    }

    // Increments game score and resets state tracking upon a goal.
    public void AddGoal(int playerWhoScored)
    {
        int pointsEarned = 1; 

        // Evaluate ball state to award double points for special attacks.
        if (theBall != null && (theBall.isFireball || theBall.isIceball))
        {
            pointsEarned = 2;
            Debug.Log("DOUBLE POINTS! SPECIAL ATTACK GOAL!");
        }

        if (playerWhoScored == 1) p1Score += pointsEarned;
        else if (playerWhoScored == 2) p2Score += pointsEarned;

        Debug.Log($"Current Score -> P1: {p1Score} | P2: {p2Score}");

        ResetStreaks();
        if (theBall != null) theBall.ResetBall();
    }

    // Increments the streak counter for Player 1 central block impacts.
    public void AddP1Hit()
    {
        p1Streak++;
        if (p1Streak >= requiredHits)
        {
            p1StreakText.text = "P1 FIREBALL!";
            p1StreakText.color = Color.red;
            
            if (theBall != null) theBall.ActivateFireball();
        }
        else { UpdateBothUIs(); }
    }

    // Increments the streak counter for Player 2 central block impacts.
    public void AddP2Hit()
    {
        p2Streak++;
        if (p2Streak >= requiredHits)
        {
            p2StreakText.text = "P2 ICEBALL!"; 
            p2StreakText.color = Color.cyan;
            
            if (theBall != null) theBall.ActivateIceball();
        }
        else { UpdateBothUIs(); }
    }

    // Tracks overall damage to the structural defense walls to evaluate win conditions.
    public void OnWallHit(bool isPlayer1Wall)
    {
        if (isPlayer1Wall) p2WallHits++;
        else p1WallHits++;

        CheckWinCondition();
    }

    // Evaluates if structural wall damage matches the win criteria.
    private void CheckWinCondition()
    {
        if (p1WallHits >= WIN_THRESHOLD) Debug.Log("Player 2 Wins!");
        if (p2WallHits >= WIN_THRESHOLD) Debug.Log("Player 1 Wins!");
    }

    public void ResetStreaks()
    {
        p1Streak = 0;
        p2Streak = 0;
        if (p1StreakText != null) p1StreakText.color = Color.green; 
        if (p2StreakText != null) p2StreakText.color = Color.green; 
        UpdateBothUIs();
    }

    private void UpdateBothUIs()
    {
        if (p1StreakText != null) p1StreakText.text = "P1 Streak: " + p1Streak + " / " + requiredHits;
        if (p2StreakText != null) p2StreakText.text = "P2 Streak: " + p2Streak + " / " + requiredHits;
    }
}