using UnityEngine;
using TMPro; 

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance; 

    [Header("Match Score (The Actual Game)")]
    public int p1Score = 0;
    public int p2Score = 0;

    [Header("Defense Streaks (The Walls)")]
    public TMP_Text p1StreakText; 
    public TMP_Text p2StreakText; 
    public int p1Streak = 0;
    public int p2Streak = 0;
    public int requiredHits = 6; 

    [Header("Object Links")]
    public Ball theBall; 

    void Awake() { Instance = this; }

    void Start() { UpdateBothUIs(); }

    public void AddGoal(int playerWhoScored)
    {
        int pointsEarned = 1; 

        // Check the Ball's memory. Was it a special attack? DOUBLE POINTS!
        if (theBall != null && (theBall.isFireball || theBall.isIceball))
        {
            pointsEarned = 2;
            Debug.Log("DOUBLE POINTS! SPECIAL ATTACK GOAL!");
        }

        // Allocate points
        if (playerWhoScored == 1) p1Score += pointsEarned;
        else if (playerWhoScored == 2) p2Score += pointsEarned;

        Debug.Log($"Current Score -> P1: {p1Score} | P2: {p2Score}");

        ResetStreaks();
        if (theBall != null) theBall.ResetBall();
    }

    public void AddP1Hit()
    {
        p1Streak++;
        if (p1Streak >= requiredHits)
        {
            p1StreakText.text = "P1 🔥 FIREBALL! 🔥";
            p1StreakText.color = Color.red;
            
            // Trigger the red state
            if (theBall != null) theBall.ActivateFireball();
        }
        else { UpdateBothUIs(); }
    }

    public void AddP2Hit()
    {
        p2Streak++;
        if (p2Streak >= requiredHits)
        {
            p2StreakText.text = "P2 ❄️ ICEBALL! ❄️"; 
            p2StreakText.color = Color.cyan;
            
            // Trigger the new icy blue state
            if (theBall != null) theBall.ActivateIceball();
        }
        else { UpdateBothUIs(); }
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