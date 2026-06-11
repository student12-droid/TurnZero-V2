using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleController : MonoBehaviour
{
    [Header("Health Bars")]
    public Image playerFill;
    public Image enemyFill;

    [Header("Selection UI")]
    public TextMeshProUGUI descriptionText;
    public GameObject selectionArrow;

    private float playerHP = 1f; 
    private float enemyHP = 1f;

    void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        playerFill.fillAmount = playerHP;
        enemyFill.fillAmount = enemyHP;
    }

    // Called when hovering over buttons
    public void ShowDescription(string desc)
    {
        descriptionText.text = desc;
    }

    // Called when an attack is selected
    public void SelectAttack(string type)
    {
        Debug.Log("Selected: " + type);
        // This triggers the Ping Pong game
    }
}
