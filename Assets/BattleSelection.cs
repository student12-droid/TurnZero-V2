// Used for selecting attack options
using UnityEngine;
using TMPro; 
using System.Collections;

public class BattleSelection : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI descriptionText; 

    [Header("Game Panels")]
    public GameObject battleUIPanel; 
    public GameObject pingPongEnvironment;  

    // variable to store the chosen attack type
    public static string chosenAttackType = "None"; 

    public const string ATTACK_HEAVY = "Heavy";
    public const string ATTACK_QUICK = "Quick";
  

    void Start()
    {
       
        // Ensure the Ping Pong environment is initially inactive when Battle UI is active
        if (pingPongEnvironment != null)
        {
            pingPongEnvironment.SetActive(false);
        }
        
        ClearDescription();
    }

    // Called when the Heavy Attack button is pressed
    public void OnHeavyAttackSelected()
    {
        chosenAttackType = ATTACK_HEAVY;
        Debug.Log("Heavy Attack Selected! Transitioning to Ping Pong.");
        StartPingPongGame();
    }

    // Called when the Quick Attack button is pressed
    public void OnQuickAttackSelected()
    {
        chosenAttackType = ATTACK_QUICK;
        Debug.Log("Quick Attack Selected! Transitioning to Ping Pong.");
        StartPingPongGame();
    }


    // Handles the transition from Battle UI to Ping Pong game
    private void StartPingPongGame()
    {
        if (battleUIPanel != null)
        {
            battleUIPanel.SetActive(false);
        }
        if (pingPongEnvironment != null)
        {
            pingPongEnvironment.SetActive(true); 
        }
        else
        {
            Debug.LogError("PingPongEnvironment not assigned in BattleSelection script!");
        }
    }

    // Handles the attack description
    public void HoverHeavy()
    {
        descriptionText.text = "<color=#00E5FF>HEAVY ATTACK:</color>\nDeals 2x Damage.\n<color=#FF4B4B>EFFECT:</color> Ball speed increases by 50%.";
    }

    public void HoverQuick()
    {
        descriptionText.text = "<color=#00E5FF>QUICK ATTACK:</color>\nSimple but Quick attack dealing light Damage.\n<color=#00FF88>EFFECT:</color> Ball speed decreases by 20%.";
    }

       public void ClearDescription()
    {
        descriptionText.text = "Select an action...";
    }
}
