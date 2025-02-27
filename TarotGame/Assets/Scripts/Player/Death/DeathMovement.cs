using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeathMovement : MonoBehaviour
{
    private TerrainGrid grid;
    private GridTransform gridTransform;
    [SerializeField]
    private Vector3Int target;
    private bool doubleMove = false;
    private int timesMoved = 0;

    public UnityEvent onExtinguish;
    public UnityEvent onTargetReached;

    private void Awake()
    {
        grid = FindObjectOfType<TerrainGrid>();
        gridTransform = GetComponent<GridTransform>();

        // normal move
        FindObjectOfType<PlayerMovement>().onPlayerMove.RemoveListener(Move);
        FindObjectOfType<PlayerMovement>().onPlayerMove.AddListener(Move);

        // chariot move
        FindObjectOfType<PlayerMovement>().onChariotMove.RemoveListener(Move);
        FindObjectOfType<PlayerMovement>().onChariotMove.AddListener(Move);
    }

    private void OnDestroy()
    {
        // clean up listeners
        FindObjectOfType<PlayerMovement>().onPlayerMove.RemoveListener(Move);
        FindObjectOfType<PlayerMovement>().onChariotMove.RemoveListener(Move);
    }

    public void Move()
    {
        doubleMove = FindObjectOfType<PlayerMovement>().ChariotMode; // update double move

        Vector3Int position = gridTransform.GetGridPosition();
        if (target == position) // no target or on target position
        {
            Terrain t = grid.CellToTerrain(position);
            foreach(GridTransform gt in t.GetResidents())
            {
                Torch torch = gt.GetComponent<Torch>();
                if (torch)
                {
                    onExtinguish?.Invoke();
                    torch.Extinguish();
                    break;
                }
            }
            onTargetReached?.Invoke();
            return;
        }

        // figure out where to move
        Vector3Int direction = position - target;

        if(Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
        {
            // move in x direction
            if (direction.x > 0)
                position.x -= 1;
            else
                position.x += 1;
        }
        else
        {
            // move in y direction
            if (direction.z > 0)
                position.z -= 1;
            else
                position.z += 1;
        }

        gridTransform.SetGridPosition(position);

        // handle double move
        if (doubleMove && timesMoved == 0)
        {
            timesMoved++;
            gridTransform.onMoveEnd.RemoveListener(Move);
            gridTransform.onMoveEnd.AddListener(Move);
        }
        else
        {
            timesMoved = 0;
            gridTransform.onMoveEnd.RemoveListener(Move);
        }
    }

    public void SetTarget(Vector3Int target) => this.target = target;
}
