using UnityEngine;

namespace TurnZero.Minigame
{
    /// <summary>
    /// Manages the central match state machine, driving turn switches and toggling active player field elements.
    /// </summary>
    public class TurnManager : MonoBehaviour
    {
        public static TurnManager Instance { get; private set; }

        [Header("Match State Matrix")]
        [Tooltip("Tracks the active turn state. True indicates Player 1, false indicates Player 2.")]
        [SerializeField] private bool isPlayer1Turn = true;

        [Header("Player 1 Environment Elements")]
        [SerializeField] private GameObject p1DefenceWall;
        [SerializeField] private GameObject p1FireballTarget;

        [Header("Player 2 Environment Elements")]
        [SerializeField] private GameObject p2DefenceWall;
        [SerializeField] private GameObject p2FireballTarget;

        // Public getter for runtime verification by external game components
        public bool IsPlayer1Turn => isPlayer1Turn;

        private void Awake()
        {
            InitializeSingleton();
        }

        private void Start()
        {
            // Set initial match configuration parameters on start
            ApplyTurnState(isPlayer1Turn);
        }

        /// <summary>
        /// Inverts the current turn index and requests a tactical reset of the ball instance.
        /// </summary>
        public void SwitchTurn()
        {
            isPlayer1Turn = !isPlayer1Turn;
            ApplyTurnState(isPlayer1Turn);

            // Execute tactical physics environment cleanup
            Ball gameBall = Object.FindFirstObjectByType<Ball>();
            if (gameBall != null)
            {
                gameBall.ResetBall();
            }
        }

        /// <summary>
        /// Explicitly shifts active game permissions to Player 1.
        /// </summary>
        public void SetPlayer1Turn()
        {
            isPlayer1Turn = true;
            ApplyTurnState(true);
        }

        /// <summary>
        /// Explicitly shifts active game permissions to Player 2.
        /// </summary>
        public void SetPlayer2Turn()
        {
            isPlayer1Turn = false;
            ApplyTurnState(false);
        }

        /// <summary>
        /// Sets field layout dependencies and structural elements active or inactive depending on state parameters.
        /// </summary>
        private void ApplyTurnState(bool p1Active)
        {
            // Handle Player 1 physical structures
            if (p1DefenceWall != null) p1DefenceWall.SetActive(p1Active);
            if (p1FireballTarget != null) p1FireballTarget.SetActive(p1Active);

            // Handle Player 2 physical structures
            if (p2DefenceWall != null) p2DefenceWall.SetActive(!p1Active);
            if (p2FireballTarget != null) p2FireballTarget.SetActive(!p1Active);

            Debug.Log($"[TurnManager] Active State Update: Player {(p1Active ? "1" : "2")}'s Phase initialized.");
        }

        private void InitializeSingleton()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }
    }
}