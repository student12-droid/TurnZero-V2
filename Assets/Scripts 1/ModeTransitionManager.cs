using UnityEngine;
using UnityEngine.SceneManagement; 

public class ModeTransitionManager : MonoBehaviour
{
    [Header("The UI Connector")]
    public GameObject dialoguePanel; 
    public GameObject interactPrompt; 

    [Header("The 3D Player")]
    public GameObject playerFPV; 
    
    [Header("Scene Travel")]
    
    public string minigameSceneName = "TurnZero_Mini Game"; 

    private bool isPlayerInRange = false;
    private bool isUIOpen = false;

    void Start()
    {
        if (interactPrompt != null) interactPrompt.SetActive(false);
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E) && !isUIOpen)
        {
            OpenUIConnector();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if (!isUIOpen && interactPrompt != null) interactPrompt.SetActive(true);
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

    // OPEN THE UI 
    void OpenUIConnector()
    {
        isUIOpen = true;
        if (interactPrompt != null) interactPrompt.SetActive(false);
        if (dialoguePanel != null) dialoguePanel.SetActive(true);

        if (playerFPV != null && playerFPV.GetComponent<FPVController>() != null)
        {
            playerFPV.GetComponent<FPVController>().enabled = false;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // --- PHASE 2: LAUNCH THE GAME ---
    public void LaunchTurnZero()
    {
        if (SceneHistory.Instance != null)
        {
            SceneHistory.Instance.SavePlayerState();
            SceneHistory.Instance.RecordScene(SceneManager.GetActiveScene().name);
            Debug.Log("Player state saved before transitioning to menu");
        }
        else
        {
            Debug.LogWarning("SceneHistory.Instance not found! Make sure SceneHistory script is in the Hub Scene.");
        }

        //This teleports the player to the other screen
        SceneManager.LoadScene(minigameSceneName);
    }
}