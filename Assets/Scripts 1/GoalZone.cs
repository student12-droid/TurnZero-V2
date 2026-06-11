using UnityEngine;

public class GoalZone : MonoBehaviour
{
    // This triggers when the ghost box catches the puck
    void OnTriggerEnter2D(Collider2D col)
    {
        // Check for the "Player" tag!
        if (col.gameObject.CompareTag("Player"))
        {
            Debug.Log("MISSED! The streak is broken!");
            
            //Score Reset
            ScoreManager.Instance.ResetStreaks();
            
            // Reset position of the ball
            col.GetComponent<Ball>().ResetBall();
        }
    }
}