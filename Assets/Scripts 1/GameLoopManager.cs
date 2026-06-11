using UnityEngine;
using System.Collections;

public class GameLoopManager : MonoBehaviour
{
    [Header("UI Setup")]
    public GameObject mainMenuCanvas;
    public GameObject pingPongEnvironment; 

    [Header("Turn Settings")]
    public float turnDuration = 30f;
    private float timer;

    public bool isGameStarted = false;
    public bool isMinigameActive = false;

    void Start()
    {
        // Initialize the turn timer
        timer = turnDuration;
        
        // Ensure the game loop starts immediately when the scene is loaded
        isGameStarted = true; 
        
        // Force this off so the Inspector checkbox can't freeze your timer
        isMinigameActive = false; 
    }

    void Update()
    {
        // Handle countdown logic when the match is active and not paused by a minigame
        if (isGameStarted && !isMinigameActive)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                ExecuteAutoAttack();
            }
        }
    }

    public void StartGameFromMenu()
    {
        if (mainMenuCanvas != null)
        {
            mainMenuCanvas.SetActive(false); // Hide the main menu UI
        }

        if (pingPongEnvironment != null)
        {
            pingPongEnvironment.SetActive(true); // Enable the ping-pong environment
        }

        isGameStarted = true;
        ResetTurn();
        Debug.Log("Game Started! Timer ticking...");
    }

    public void StartMinigame()
    {
        isMinigameActive = true;
        Debug.Log("Switching to Ping-Pong Minigame!");
    }

    public void ResetTurn()
    {
        timer = turnDuration;
        isMinigameActive = false;
    }

    void ExecuteAutoAttack()
    {
        Debug.Log("Time's up! Attack launched automatically.");
        
        // Notify the Turn Manager to swap active player states and wall defenses
        if (TurnManager.Instance != null)
        {
            TurnManager.Instance.SwitchTurn();
        }

        ResetTurn();
    }
}