using UnityEngine;
using TMPro; // Needed for the dialogue text

public class NPCInteraction : MonoBehaviour
{
    [Header("UI & Dialogue")]
    public GameObject interactPrompt; // "Press E to Talk"
    public GameObject dialoguePanel;  // The background box for the dialogue
    public TextMeshProUGUI dialogueText;
    public string npcMessage = "Let's settle this on the neon table. Get ready!";

    [Header("Game Switching")]
    public GameObject playerFPV; // 3D Player Capsule
    public GameObject turnZeroMinigame; // The parent object holding the 2D game

    private bool isPlayerInRange = false;
    private bool isTalking = false;

    void Start()
    {
        // Make sure UI is hidden when the game starts
        if (interactPrompt != null) interactPrompt.SetActive(false);
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
    }

    void Update()
    {
        // If the player is close and presses 'E'
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!isTalking)
            {
                StartDialogue();
            }
            else
            {
                // If they press E again while talking, launch the game
                StartMinigame();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the zone is tagged "Player"
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if (!isTalking && interactPrompt != null) interactPrompt.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (interactPrompt != null) interactPrompt.SetActive(false);
        }
    }

    void StartDialogue()
    {
        isTalking = true;
        if (interactPrompt != null) interactPrompt.SetActive(false);
        if (dialoguePanel != null) dialoguePanel.SetActive(true);
        
        if (dialogueText != null) dialogueText.text = npcMessage;

        // Freeze the FPV camera and movement so they can't walk away mid-conversation
        playerFPV.GetComponent<FPVController>().enabled = false;
    }

    void StartMinigame()
    {
        if (dialoguePanel != null) dialoguePanel.SetActive(false);

        // Turn off the 3D player completely (this disables the 3D camera)
        playerFPV.SetActive(false);

        // CRITICAL: Unlock the mouse so you can click the minigame UI!
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Turn on the Turn Zero game and 2D camera!
        turnZeroMinigame.SetActive(true);
    }
}