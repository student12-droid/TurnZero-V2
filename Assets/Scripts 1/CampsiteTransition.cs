using UnityEngine;

public class LevelTravelNPC : MonoBehaviour
{
    public GameObject floatingText;
    public LevelFader fader; 
    
    [Header("Destination")]
    [Tooltip("Demo_2")]
    public string sceneToLoad = "Demo_2";

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

            // Locks the FPV camera
            
            //Fade to black and go directly to the Campsite
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