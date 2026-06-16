using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalZone : MonoBehaviour
{
    [Header("Defense System Links")]
    [Tooltip("The specific defense wall protecting this goal.")]
    public DefenseWall protectingWall;

    [Header("Goal Configuration")]
    [Tooltip("Check this box if this is Player 1's goal zone. Uncheck it for Player 2.")]
    public bool isPlayer1Goal;

    private void OnTriggerEnter2D(Collider2D col)
    {
        // Verify the colliding object is explicitly tagged as the Ball
        if (col.gameObject.CompareTag("Ball"))
        {
            if (protectingWall == null)
            {
                Debug.LogError($"GoalZone on {gameObject.name} is missing its DefenseWall reference!");
                return;
            }

            // Execute game over sequence only if the protecting defense wall is destroyed
            if (protectingWall.IsDestroyed())
            {
                ExecuteGameOver();
            }
            else
            {
                Debug.Log($"Ball hit goal boundary, but {protectingWall.name} is still standing.");
            }
        }
    }

    private void ExecuteGameOver()
    {
        // Clear active scoring streaks via the central manager.
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ResetStreaks();
        }

        // Output results to console based on which boundary layer was crossed
        if (isPlayer1Goal)
        {
            Debug.Log("CRITICAL HIT: Player 1's goal line was breached! Player 2 Wins!");
        }
        else
        {
            Debug.Log("CRITICAL HIT: Player 2's goal line was breached! Player 1 Wins!");
        }

        // Load the main game scene using its explicit Build Index (0)
        SceneManager.LoadScene(0);
    }
}