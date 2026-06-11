using UnityEngine;

public class CareerNPCInteract : MonoBehaviour
{
    public GameObject floatingText;
    public LevelFader fader; 
    
    [Header("Destination")]
    [Tooltip("Type the exact name of the Career scene here!")]
    public string sceneToLoad = "SampleScene";

    private bool playerInRange = false;
    private bool isTransitioning = false;

    void Start()
    {
        if (floatingText != null) floatingText.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !isTransitioning)
        {
            isTransitioning = true;
            if (floatingText != null) floatingText.SetActive(false);
            
            // Free the mouse pointer so you can click the UI
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            
            // Fade to black and go directly to the target scene
            if (fader != null) fader.FadeToScene(sceneToLoad);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTransitioning)
        {
            playerInRange = true;
            if (floatingText != null) floatingText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (floatingText != null) floatingText.SetActive(false);
        }
    }
}