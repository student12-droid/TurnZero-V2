//Saves your state in Scenes
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneHistory : MonoBehaviour
{
    public static SceneHistory Instance;
    [Header("Scene Tracking")]
    public string previousSceneName = "";

    [Header("Player State")]
    public Vector3 savedPlayerPosition;
    public Quaternion savedPlayerRotation;
    public bool hasStoredPosition = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene loaded: " + scene.name + " | hasStoredPosition: " + hasStoredPosition);
        // Restores player position when returning to Hub World
        if (scene.name == "TurnZero_Main Game" && hasStoredPosition)
        {
            StartCoroutine(RestoreAfterFrame());
        }
    }

    IEnumerator RestoreAfterFrame()
    {
        GameObject player = null;
        float timeout = 5f;
        float elapsed = 0f;

        while (player == null && elapsed < timeout)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                elapsed += Time.deltaTime;
                yield return null; // keep trying every frame
            }
        }

        if (player != null)
        {
            RestorePlayerPosition();
        }
        else
        {
            Debug.LogWarning("Timed out waiting for Player to spawn!");
        }
    }
    public void RecordScene(string currentScene)
    {
        previousSceneName = currentScene;
        Debug.Log("Recorded previous scene: " + currentScene);
    }

    // Called before leaving the hub world to save position
    public void SavePlayerState()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            savedPlayerPosition = player.transform.position;
            savedPlayerRotation = player.transform.rotation;
            hasStoredPosition = true;

            Debug.Log("Player state saved at position: " + savedPlayerPosition);
        }
        else
        {
            Debug.LogWarning("Could not find Player to save state!");
        }
    }

    // This gets called when the hub loads to restore the player position
    void RestorePlayerPosition()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log("RestorePlayerPosition called. Player found: " + (player != null));

        if (player != null)
        {
            // Disables the character controller to avoid conflict
            CharacterController cc = player.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;
            Debug.Log("Moving player to: " + savedPlayerPosition);
            player.transform.position = savedPlayerPosition;
            player.transform.rotation = savedPlayerRotation;

            // Restore position and rotation
            player.transform.position = savedPlayerPosition;
            player.transform.rotation = savedPlayerRotation; // fixed: was a duplicate position line

            // Re-enables the character controller
            if (cc != null) cc.enabled = true;
            Debug.Log("Player position restored to: " + savedPlayerPosition);
            hasStoredPosition = false;

            // Clear the flag to prevent restore again on accidental reload
            hasStoredPosition = false;
        }
        else
        {
            Debug.LogWarning("Could not find Player to restore position!");
        }
    }

    public void ClearStoredPosition()
    {
        hasStoredPosition = false;
    }
}