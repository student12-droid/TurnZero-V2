public static class LevelManager
{
    // Default config sets max hits to Level 1 parameters
    public static int CurrentWallTargetHits = 15;

    // Updates the shared configuration based on the target level identifier
    public static void SetLevelDifficulty(int level)
    {
        if (level == 2)
        {
            CurrentWallTargetHits = 20;
        }
        else if (level == 3)
        {
            CurrentWallTargetHits = 25;
        }
        else
        {
            CurrentWallTargetHits = 15; // Level 1 or fallback default
        }
    }
}