using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class Shadow : MonoBehaviour
{
    private GridTransform gridTransform;
    private GridCollider gridCollider;
    private Interactor interactor;
    private TerrainGrid grid;

    private MirrorSwapScript mirrorSwap;
    private ToggleShadow toggleShadow;

    [SerializeField]
    private Sprite moonCard;

    public UnityEvent onShadowMove;
    public UnityEvent onShadowSwap;
    public UnityEvent<Vector2> onMoveDirectional;
    public UnityEvent<Vector2> onPushDirectional;

    private Grabbable held;

    private void Awake()
    {
        gridTransform = GetComponent<GridTransform>();
        gridCollider = GetComponent<GridCollider>();
        interactor = GetComponent<Interactor>();
        grid = FindObjectOfType<TerrainGrid>();
        mirrorSwap = GetComponent<MirrorSwapScript>();
        toggleShadow = GetComponent<ToggleShadow>();
        FindObjectOfType<StateChange>().onStateChange.RemoveListener(() => { if (gameObject.activeInHierarchy) mirrorSwap.Swap(); });
        FindObjectOfType<StateChange>().onStateChange.AddListener(() => { if (gameObject.activeInHierarchy) mirrorSwap.Swap(); });
        SetUpAbilityButton();

        FindObjectOfType<PlayLoadedLevelV2>().onLoadEndedLate.AddListener(LateLoad);
    }

    private void LateLoad()
    {
        FindObjectOfType<PlayLoadedLevelV2>().onLoadEndedLate.RemoveListener(LateLoad);
        FindObjectOfType<PlayerMovement>().UpdateShadow();
        toggleShadow.Init();

        // disables shadow
        toggleShadow.Toggle();
        toggleShadow.Toggle();
    }

    private void SetUpAbilityButton()
    {
        AbilityButtonHandler abh = FindObjectOfType<AbilityButtonHandler>();
        abh.SetSprite(moonCard);
        abh.onPress.RemoveListener(() => toggleShadow.Toggle());
        abh.onPress.AddListener(() => toggleShadow.Toggle());
        abh.EnableButton(true);
    }

    private void OnDestroy()
    {
        AbilityButtonHandler abh = GetComponent<AbilityButtonHandler>();
        if (abh)
            abh.onPress.RemoveListener(() => toggleShadow.Toggle());
        FindObjectOfType<StateChange>().onStateChange.RemoveListener(() => { if (gameObject.activeInHierarchy) mirrorSwap.Swap(); });
    }

    public void Interact() => interactor.Interact();

    public void Move(Vector3Int offset)
    {
        Vector2 direction = new(offset.z, -offset.x);

        Vector3Int pos = gridTransform.GetGridPosition();
        pos += offset;
        if (gridTransform.SetGridPosition(pos))
        {
            onShadowMove?.Invoke();
            onMoveDirectional?.Invoke(new(-direction.x, direction.y));
        }

        Terrain target = grid.CellToTerrain(pos);
        if (target != null && target.GetResidents().Count != 0)
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
                            break;
                        }
                }
            }
        }

    }

    public void DropHeldItem()
    {
        if (held != null)
        {
            held.Interact(interactor);
            held = null;
            gridTransform.onMoveEnd.RemoveListener(DropHeldItem);
        }
    }

    public GridCollider.CollisionType CheckCollision(Vector3Int offset)
    {
        Vector3Int pos = gridTransform.GetGridPosition();
        return gridCollider.CheckForCollision(pos + offset);
    }


    public void SwapPosition(Vector3Int newPos)
    {
        gridTransform.ForceSetGridPosition(newPos);
        onShadowSwap?.Invoke();
    }
}
