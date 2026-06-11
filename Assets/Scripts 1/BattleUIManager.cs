using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class BattleUIManager : MonoBehaviour
{
    public static BattleUIManager Instance;

    [Header("Health Bars")]
    public Slider p1HealthBar;
    public Slider p2HealthBar;
    public float maxHealth = 100f;
    private float p1CurrentHealth;
    private float p2CurrentHealth;

    [Header("Menu Elements")]
    public GameObject attackMenuContainer;
    public TextMeshProUGUI descriptionTextBox;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        p1CurrentHealth = maxHealth;
        p2CurrentHealth = maxHealth;
        UpdateHealthUI();

        if (descriptionTextBox != null)
        {
            descriptionTextBox.text = "Select an attack to view details.";
        }
    }

    public void TakeDamage(int playerNumber, float damageAmount)
    {
        if (playerNumber == 1)
        {
            p1CurrentHealth -= damageAmount;
            if (p1CurrentHealth < 0) p1CurrentHealth = 0;
        }
        else if (playerNumber == 2)
        {
            p2CurrentHealth -= damageAmount;
            if (p2CurrentHealth < 0) p2CurrentHealth = 0;
        }
        
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (p1HealthBar != null) p1HealthBar.value = p1CurrentHealth / maxHealth;
        if (p2HealthBar != null) p2HealthBar.value = p2CurrentHealth / maxHealth;
    }

    // Button Event: Quick Attack
    public void OnQuickAttackSelected()
    {
        if (descriptionTextBox != null)
        {
            descriptionTextBox.text = "QUICK ATTACK:\nStandard strike. Normal damage.\nEFFECT: Ball speed decreases by 20%.";
        }   
    }

    // Button Event: Heavy Attack
    public void OnHeavyAttackSelected()
    {
        if (descriptionTextBox != null)
        {
            descriptionTextBox.text = "HEAVY ATTACK:\nDeals 2x Damage.\nEFFECT: Ball speed increases by 50%.";
        }
    }
}