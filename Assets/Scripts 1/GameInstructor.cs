using UnityEngine;
using TMPro;

public class GameInstructor : MonoBehaviour
{
    [Header("UI Components")]
    [Tooltip("The TextMeshPro element used to display instructions on the screen.")]
    public TextMeshProUGUI instructionText;

    [Header("Display Settings")]
    [Tooltip("Duration in seconds before the instruction text is hidden.")]
    public float displayDuration = 5f;

    void Start()
    {
        if (instructionText == null)
        {
            Debug.LogError($"GameInstructor on {gameObject.name} is missing its TextMeshProUGUI reference!");
            return;
        }

        // Initialize on-screen game brief with proximity interaction rules.
        instructionText.text = "WELCOME TO TURN ZERO!\n\n" +
                               "MOVEMENT: Use WASD to navigate the map.\n" +
                               "INTERACTION: Walk up to an NPC robot to initiate the ping-pong match two players are needed to play the pingpong.\n" +
                               "MATCH CONTROLS: Player 1 (WASD) | Player 2 (Arrow Keys).\n" +
                               "OBJECTIVE: Accumulate 15 structural wall hits to win.\n" +
                               "POWER-UP: Strike the central block 6 times to trigger special attributes.";
        
        // Schedule UI cleanup
        Invoke(nameof(HideInstructions), displayDuration);
    }

    private void HideInstructions()
    {
        if (instructionText != null)
        {
            instructionText.gameObject.SetActive(false);
        }
    }
}