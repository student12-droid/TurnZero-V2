using UnityEngine;

namespace TurnZero.Minigame
{
    /// <summary>
    /// Processes physical locomotion, input tracking, and zone boundaries for a paddle unit.
    /// Can be configured in the Inspector for either Player 1 or Player 2.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class PaddleController : MonoBehaviour
    {
        [Header("Player Configuration")]
        [Tooltip("Check this if it's Player 1. Uncheck for Player 2.")]
        [SerializeField] private bool isPlayer1 = true;

        [Header("Input Bindings")]
        [SerializeField] private KeyCode upKey = KeyCode.W;
        [SerializeField] private KeyCode downKey = KeyCode.S;
        [SerializeField] private KeyCode leftKey = KeyCode.A;
        [SerializeField] private KeyCode rightKey = KeyCode.D;

        [Header("State Variables")]
        [Tooltip("The TurnManager flips this to true when the defense wall spawns.")]
        public bool isDefenseWallActive = false;

        [Header("Locomotion Metrics")]
        [SerializeField] private float moveSpeed = 15f;

        [Header("Spatial Domain Boundaries")]
        [SerializeField] private float minX = -3.6f;
        [SerializeField] private float maxX = 3.4f;
        [SerializeField] private float minY = -5.28f;
        [SerializeField] private float maxY = 8.08f;
        [Tooltip("The separation partition of the arena layout.")]
        [SerializeField] private float centerLineY = 1.3203f;

        private Rigidbody2D rb;
        private Vector2 movementInput;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            EstablishInitialPosition();
        }

        private void Update()
        {
            // If it's not our turn, or if that white defense block is up, drop the inputs and freeze.
            if (!IsOurTurn() || isDefenseWallActive)
            {
                HaltVelocityBuffer();
                return;
            }

            ProcessInputCoordinates();
        }

        private void FixedUpdate()
        {
            // Double check we are actually allowed to move the physics body this frame
            if (!IsOurTurn() || isDefenseWallActive) return;

            ExecuteLocomotionEngine();
        }

        private bool IsOurTurn()
        {
            // Failsafe in case the TurnManager isn't loaded in the scene yet
            if (TurnManager.Instance == null) return false;

            // Player 1 moves on P1's turn, Player 2 moves on P2's turn. Simple.
            return isPlayer1 ? TurnManager.Instance.IsPlayer1Turn : !TurnManager.Instance.IsPlayer1Turn;
        }

        private void ProcessInputCoordinates()
        {
            movementInput = Vector2.zero;

            // Using the keys assigned in the Unity Inspector
            if (Input.GetKey(upKey)) movementInput.y = 1f;
            if (Input.GetKey(downKey)) movementInput.y = -1f;
            if (Input.GetKey(leftKey)) movementInput.x = -1f;
            if (Input.GetKey(rightKey)) movementInput.x = 1f;
        }

        private void ExecuteLocomotionEngine()
        {
            Vector2 displacementDelta = movementInput.normalized * moveSpeed * Time.fixedDeltaTime;
            Vector2 projectedPosition = rb.position + displacementDelta;

            // Enforce hard clamps. Everyone shares the same X boundaries.
            projectedPosition.x = Mathf.Clamp(projectedPosition.x, minX, maxX);

            // Clamp Y based on whose side of the court we belong to
            if (isPlayer1)
            {
                projectedPosition.y = Mathf.Clamp(projectedPosition.y, minY, centerLineY);
            }
            else
            {
                projectedPosition.y = Mathf.Clamp(projectedPosition.y, centerLineY, maxY);
            }

            rb.MovePosition(projectedPosition);
        }

        private void EstablishInitialPosition()
        {
            // Player 1 spawns at the bottom, Player 2 spawns at the top
            Vector2 originVector = isPlayer1 ? new Vector2(0f, minY) : new Vector2(0f, maxY);
            transform.position = originVector;
            rb.position = originVector;
        }

        private void HaltVelocityBuffer()
        {
            movementInput = Vector2.zero;
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
    }
}