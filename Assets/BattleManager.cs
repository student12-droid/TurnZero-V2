// BattleManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System;

public class BattleManager : MonoBehaviour
{
    [Header("Player Stats")]
    public float player1HP = 100f;
    public float player2HP = 100f;
    public float player1XP = 100f;
    public float player2XP = 100f;

    [Header("UI Elements")]
    public Slider player1HPBar;
    public Slider player2HPBar;
    public TextMeshProUGUI player1HPText;
    public TextMeshProUGUI player2HPText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI turnText;
    public GameObject attackPanel;
    public TextMeshProUGUI resultText;
    public GameObject resultPanel;

    [Header("Settings")]
    public float turnTime = 30f;
    public string pingPongScene = "PingPongScene";

    private float timer;
    private bool isPlayer1Turn = true;
    private bool waitingForAttack = true;
    private string selectedAttack = "";

    // Static so PingPong scene can write the result back
    public static bool lastMinigameWon = false;
    public static string pendingAttack = "";
    public static bool returningFromMinigame = false;

    void Start()
    {
        // Check if we're returning from the minigame
        if (returningFromMinigame)
        {
            returningFromMinigame = false;
            StartCoroutine(HandleMinigameResult());
        }
        else
        {
            StartTurn();
        }
    }

    void Update()
    {
        if (!waitingForAttack) return;

        timer -= Time.deltaTime;
        UpdateTimerUI();

        if (timer <= 0)
        {
            // Time ran out — skip turn
            ShowResult("Time's up! Turn skipped.");
            StartCoroutine(NextTurn(1.5f));
        }
    }

    private void StartCoroutine(object v)
    {
        throw new NotImplementedException();
    }

    void StartTurn()
    {
        timer = turnTime;
        waitingForAttack = true;
        attackPanel.SetActive(true);
        resultPanel.SetActive(false);

        string currentPlayer = isPlayer1Turn ? "Player 1" : "Player 2";
        turnText.text = currentPlayer + "'s Turn";
    }

    void UpdateTimerUI()
    {
        int seconds = Mathf.CeilToInt(timer);
        timerText.text = seconds.ToString();

        // Turn timer red when under 10 seconds
        timerText.color = timer <= 10f ? Color.red : Color.white;
    }

    // Called by UI buttons
    public void OnHeavyAttack()
    {
        SelectAttack("Heavy");
    }

    public void OnLightAttack()
    {
        SelectAttack("Light");
    }

    public void OnSpecialAttack()
    {
        SelectAttack("Special");
    }

    void SelectAttack(string attack)
    {
        selectedAttack = attack;
        pendingAttack = attack;
        waitingForAttack = false;
        attackPanel.SetActive(false);

        // Launch the ping pong minigame
        SceneManager.LoadScene(pingPongScene);
    }

    IEnumerator HandleMinigameResult()
    {
        attackPanel.SetActive(false);
        resultPanel.SetActive(true);

        if (lastMinigameWon)
        {
            // Attack lands — deal damage
            float damage = GetDamage(pendingAttack);
            if (isPlayer1Turn)
            {
                player2HP -= damage;
                player2HP = Mathf.Max(player2HP, 0);
                UpdateHPBars();
                ShowResult("Attack lands! " + damage + " damage!");

                // XP changes
                player1XP *= 1.05f;
                player2XP *= 0.95f;
            }
            else
            {
                player1HP -= damage;
                player1HP = Mathf.Max(player1HP, 0);
                UpdateHPBars();
                ShowResult("Attack lands! " + damage + " damage!");

                player2XP *= 1.05f;
                player1XP *= 0.95f;
            }

            // Check win condition
            if (player1HP <= 0 || player2HP <= 0)
            {
                yield return new WaitForSeconds(1.5f);
                EndGame();
                yield break;
            }
        }
        else
        {
            ShowResult("Attack missed!");
        }

        yield return new WaitForSeconds(1.5f);
        isPlayer1Turn = !isPlayer1Turn;
        StartTurn();
    }

    float GetDamage(string attack)
    {
        switch (attack)
        {
            case "Heavy":   return 25f;
            case "Light":   return 10f;
            case "Special": return 40f;
            default:        return 10f;
        }
    }

    void UpdateHPBars()
    {
        player1HPBar.value = player1HP / 100f;
        player2HPBar.value = player2HP / 100f;
        player1HPText.text = Mathf.RoundToInt(player1HP) + " HP";
        player2HPText.text = Mathf.RoundToInt(player2HP) + " HP";
    }

    void ShowResult(string message)
    {
        resultText.text = message;
        resultPanel.SetActive(true);
    }

    IEnumerator NextTurn(float delay)
    {
        yield return new WaitForSeconds(delay);
        isPlayer1Turn = !isPlayer1Turn;
        StartTurn();
    }

    void EndGame()
    {
        string winner = player1HP > 0 ? "Player 1 Wins!" : "Player 2 Wins!";
        ShowResult(winner);
        attackPanel.SetActive(false);
        // Later: load a game over screen
    }
}