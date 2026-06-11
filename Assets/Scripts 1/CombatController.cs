using UnityEngine;

public class CombatController : MonoBehaviour
{
    public int perfectStreak = 0;
    public float scoreBonus = 1.0f;

    public void OnBallHit()
    {
        perfectStreak++;
        if (perfectStreak == 6)
        {
            ApplyPerfectBonus();
        }
    }

    void ApplyPerfectBonus()
    {
        scoreBonus += 0.5f; // Increase damage multiplier
        Debug.Log("6 Perfect Hits! Damage Boost Active. Bonus: " + scoreBonus);
    }

    public void ResetStreak()
    {
        perfectStreak = 0;
        scoreBonus = 1.0f; // Reset bonus alongside streak
    }
}