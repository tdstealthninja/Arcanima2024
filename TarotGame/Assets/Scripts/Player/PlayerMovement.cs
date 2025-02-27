using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.UI;
using Cinemachine;

[RequireComponent(typeof(GridTransform))]
public class PlayerMovement : MonoBehaviour
{
    [HideInInspector]
    public bool moveEnabled = true;

    private GridTransform gridTransform;
    private GridCollider gridCollider;
    private TerrainGrid grid;
    private Interactor interactor;
    [SerializeField]private GameObject tutorialObj;

    // abilities
    private Shadow shadow;
    public bool allowChariotMode = false;
    private bool chariotMode = false;

    public bool ChariotMode { get { return chariotMode; } private set { chariotMode = value; } }

    private readonly float movementCooldown = 0.01f;
    private Vector3 playerInput;
    private bool canMove = true;
    private bool firstLevel = true;
    private Grabbable held = null;

    [Header("Events")]
    public UnityEvent onPlayerMove;
    public UnityEvent onChariotMove;
    public UnityEvent<Vector2> onPlayerMoveDirectional;
    public UnityEvent onPlayerSwap;
    public UnityEvent onPush;
    public UnityEvent<Vector2> onPushDirectional;

    private void Awake()
    {
        StartCoroutine(WaitForInstance());

        if (FindObjectOfType<SaveGTDataManagerV2>().GetCurrentLevelObject().name != "TutorialGeneral1")
            tutorialObj.SetActive(false);
        
        gridTransform = GetComponent<GridTransform>();
        gridCollider = GetComponent<GridCollider>();
        grid = FindObjectOfType<TerrainGrid>();
        interactor = GetComponent<Interactor>();

        var virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        virtualCamera.Follow = transform;
        virtualCamera.LookAt = transform;

        FindObjectOfType<StateChange>().onStateChange.RemoveListener(OnStateChange);
        FindObjectOfType<StateChange>().onStateChange.AddListener(OnStateChange);

        UpdateShadow();
    }

    public void UpdateShadow()
    {
        shadow = FindObjectOfType<Shadow>();
        if (!shadow)
            return;
    }

    private void OnStateChange()
    {
        if (allowChariotMode)
            chariotMode = !chariotMode;
    }

    public void GatherInput(InputAction.CallbackContext context) => Move(context.ReadValue<Vector2>().normalized, false);

    public void Move(Vector2 direction) => Move(direction, true);
    private void Move(Vector2 direction, bool touch)
    {
        // Getting Input
        if (canMove && moveEnabled)
        {
            playerInput = new Vector3(direction.x, 0, direction.y);

            if (touch)
            {
                // rotates the swipe orientation to match the camera
                playerInput = Quaternion.Euler(0, 45, 0) * playerInput;

                // parse touch controls into one direction (cannot move diagonal)
                if (Mathf.Abs(playerInput.x) > Mathf.Abs(playerInput.z))
                    playerInput.z = 0;
                else if (Mathf.Abs(playerInput.z) > Mathf.Abs(playerInput.x))
                    playerInput.x = 0;
                else
                    return; // perfect diagonal swipe (rare)
            }
            Vector3Int playerMove = Vector3Int.RoundToInt(playerInput);

            // move shadow if enabled, only move player if shadow allows
            if (shadow != null && shadow.isActiveAndEnabled && !MoveShadow(playerMove))
                return;

            // Makes the player wait to move
            canMove = false;

            Vector3Int targetPos = gridTransform.GetGridPosition() + playerMove;
            Terrain target = grid.CellToTerrain(targetPos);

            if (Interact(target)) // try to interact
            {
                StartCoroutine(MovementCooldown()); // wait
                return; // don't move
            }

            if (chariotMode) // did not interact and chariot mode is on
            {
                // find furthes free space in a line
                Vector3Int testPos = targetPos;
                while (gridCollider.CheckForCollision(testPos) == GridCollider.CollisionType.None)
                {
                    targetPos = testPos;
                    testPos += playerMove;
                }
            }

            float dist = (targetPos - gridTransform.GetGridPosition()).magnitude;

            // move
            if (gridTransform.SetGridPosition(targetPos, (chariotMode ? 0.1f : gridTransform.Speed) * dist))
            {
                if (chariotMode)
                    onChariotMove?.Invoke();
                else
                    onPlayerMove?.Invoke();

                onPlayerMoveDirectional?.Invoke(direction);
            }

            tutorialObj.SetActive(false);

            // Always wait
            StartCoroutine(MovementCooldown());
        }
    }

    private bool Interact(Terrain target)
    {
        if (target != null && target.GetResidents().Count != 0) // move single space
        {
            foreach (GridTransform resident in target.GetResidents())
            {
                // try to interact with the first interactable on the tile that succeeds
                Interactable interactable = resident.GetComponent<Interactable>();
                if (interactable)
                {
                    GridCollider interactableGridCollider = interactable.GetComponent<GridCollider>();
                    // interact if there is no collider or if it will collide with it
                    if (!interactableGridCollider || (interactableGridCollider.CollidesWithMe & gridCollider.ICollideWith) != 0)
                        if (interactable.Interact(interactor))
                        {
                            return true;
                        }
                }
            }
        }

        return false;
    }

    private void DropHeldItem()
    {
        held.Interact(interactor);
        held = null;
        gridTransform.onMoveEnd.RemoveListener(DropHeldItem);
    }

    /// <summary>
    /// Moves the shadow based on collisions.
    /// </summary>
    /// <returns>Returns if the player should move.</returns>
    private bool MoveShadow(Vector3Int offset)
    {
        // set up shadow offset
        Vector3Int shadowOffset = -offset;

        // get collisions
        Vector3Int shadowPos = shadow.GetComponent<GridTransform>().GetGridPosition();
        GridCollider.CollisionType shadowCollision = shadow.CheckCollision(shadowOffset);
        Vector3Int pos = gridTransform.GetGridPosition();
        GridCollider.CollisionType playerCollsion = gridCollider.CheckForCollision(pos + offset);

        // at least one is at the mirror
        if (playerCollsion == GridCollider.CollisionType.Interactable || shadowCollision == GridCollider.CollisionType.Interactable)
        {
            if (playerCollsion == GridCollider.CollisionType.Interactable)
            {
                return true; // player activates mirror, shadow doesn't move
            }
            else
            {
                shadow.Move(shadowOffset); // shadow activates mirror, player doesn't move
                return false;
            }
        }
        // one is blocked
        else if (playerCollsion == GridCollider.CollisionType.Blocked || shadowCollision == GridCollider.CollisionType.Blocked)
            return false; // Blocked and cannot move
        else if (playerCollsion == GridCollider.CollisionType.None && shadowCollision == GridCollider.CollisionType.None)
        {
            // moving onto the same tile
            if (shadowPos + shadowOffset == pos + offset)
                return true;

            // free to move
            shadow.Move(shadowOffset); // move shadow
            return true; // move player
        }
        else // at least one is pushable
        {
            if (shadowCollision == GridCollider.CollisionType.Pushable)
            {
                shadow.Move(shadowOffset); // move shadow (push object)

                if (playerCollsion == GridCollider.CollisionType.Pushable) // can player push?
                    return true; // yes
                else
                    return false; // no (don't move)
            }

            else
                return true; // shadow cannot push so that player must be able to push
        }
    }

    private IEnumerator WaitForInstance()
    {
        while (SwipeDetection.Instance == null)
            yield return null;

        SwipeDetection.Instance.onSwipe += Move;
    }

    private IEnumerator MovementCooldown()
    {
        yield return new WaitForSeconds(movementCooldown); // waits movementCooldown seconds
        canMove = true;
    }

    public void SwapPosition(Vector3Int newPos)
    {
        gridTransform.ForceSetGridPosition(newPos);
        onPlayerSwap?.Invoke();
    }
}
