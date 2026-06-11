using UnityEngine;

public class PersistentPlayer : MonoBehaviour
{
    // This creates a static memory of our player
    public static PersistentPlayer Instance;

    void Awake()
    {
        // 1. If there is no player, make this the player and don't destroy it.
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // The VIP Pass! Unity will not destroy this.
        }
        // 2. Check for player duplicate and deletes it.
        else
        {
            Destroy(gameObject); 
        }
    }
}