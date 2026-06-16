using UnityEngine;

public class MinigameInitializer : MonoBehaviour
{
    [Header("Scene Components")]
    public DefenseWall player1Wall;
    public DefenseWall player2Wall;

    void Awake()
    {
        // Extract the cached target hits from the global level tracking state.
        int targetHits = LevelManager.CurrentWallTargetHits;
        Debug.Log($"Initializer executing. Applying {targetHits} max hits to active defensive walls.");

        // Override default limits and force a visual UI refresh for Player 1's defense wall.
        if (player1Wall != null) 
        {
            player1Wall.maxHits = targetHits;
            player1Wall.UpdateHealthUI(); 
        }
        
        // Override default limits and force a visual UI refresh for Player 2's defense wall.
        if (player2Wall != null) 
        {
            player2Wall.maxHits = targetHits;
            player2Wall.UpdateHealthUI(); 
        }
    }
}