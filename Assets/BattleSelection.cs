// Transitions between Battle panel and PingPong with hover descriptions
using UnityEngine;
using TMPro;

public class BattleSelection : MonoBehaviour
{
    [Header("Panels")]
    public GameObject fromPanel;
    public GameObject toPanel;

    [Header("UI Elements")]
    public TextMeshProUGUI descriptionText;

    void Start()
    {
        ClearDescription();
    }

    public void OnButtonPressed()
    {
        if (fromPanel != null)
            fromPanel.SetActive(false);

        if (toPanel != null)
            toPanel.SetActive(true);
        else
            Debug.LogError("ToPanel not assigned in PanelTransition script!");
    }

    public void HoverEnter() //Displays text when you hover over the button
    {
        if (descriptionText != null)
            descriptionText.text = "Start the Battle for your Freedom";
    }

    public void HoverExit()
    {
        ClearDescription();
    }

    public void ClearDescription()
    {
        if (descriptionText != null)
            descriptionText.text = "Select an action...";
    }
}