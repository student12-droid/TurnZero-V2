using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("UI References")]
    public GameObject pauseMenuPanel;
    public GameObject mainMenuPanel;
    public GameObject MainGame;


    [Header("Settings")]
    public KeyCode pauseKey = KeyCode.Escape;

    private bool isPaused = false;

    void Start()
    {
        // Make sure the menu starts hidden
        pauseMenuPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
            Pause();
        else
            Resume();
    }

    public void Pause()
    {
        isPaused = true;
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;         // Freeze game time
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume()
    {
        isPaused = false;
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;         // Restore game time
        Cursor.lockState = CursorLockMode.Locked;  // Remove if you don't use cursor lock
        Cursor.visible = false;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f; 
        pauseMenuPanel.SetActive(false); //Stops showing the pause menu
        MainGame.SetActive(false); //Hides the main game
        mainMenuPanel.SetActive(true); //Shows the main menu
        isPaused = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}