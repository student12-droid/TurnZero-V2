using UnityEngine;

public class WallManager : MonoBehaviour
{
    [Header("Assign both walls here")]
    public DefenseWall wall1;
    public DefenseWall wall2;

    public void ResetBothWalls()
    {
        if (wall1 != null) wall1.ResetWall();
        if (wall2 != null) wall2.ResetWall();
    }
}