using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GridTransform : MonoBehaviour
{
    private bool moving = false;

    public bool IsMoving { get => moving; }

    [SerializeField]
    private Vector3Int gridPosition;
    [SerializeField, Tooltip("Number of seconds to perform move.")]
    private float defaultMoveSpeed = 0.25f; // seconds to perform move
    private static TerrainGrid grid;
    private Vector3 offset = new(0, 1, 0);

    public float Speed { get { return defaultMoveSpeed; } }

    [Header("Events")]
    public UnityEvent onMoveStart;
    public UnityEvent onMoveDurring;
    public UnityEvent onMoveEnd;

    public UnityEvent<GridTransform, Vector3Int, Vector3Int> onMoveSuccess;

    private void Awake()
    {

    }

    private void Start()
    {
        Build();
    }

    private void OnDestroy()
    {
        // prevents issues with null reference
        Terrain current = grid?.CellToTerrain(gridPosition);
        current?.RemoveResident(this);
    }

    #region Getters and Setters

    public static TerrainGrid GetTerrainGrid() => grid;

    // Only set terrain grid if it is null (prevents GridTransforms on multiple TerrainGrids)
    public static void SetTerrainGrid(TerrainGrid terrainGrid) => grid = terrainGrid;

    /// <summary>
    /// This sets the position without moving or checking any collision.
    /// </summary>
    /// <param name="pos"></param>
    public void ForceSetGridPosition(Vector3Int pos)
    {
        grid.CellToTerrain(GetGridPosition())?.RemoveResident(this);
        gridPosition = pos;
        Terrain target = grid.CellToTerrain(gridPosition);
        if (target != null)
        {
            transform.position = target.GetCellPosition() + offset;
            target.AddResident(this);
        }
    }

    public Vector3Int GetGridPosition() => gridPosition;

    public bool SetGridPosition(Vector3Int targetPos) => SetGridPosition(targetPos, defaultMoveSpeed);
    public bool SetGridPosition(Vector3Int targetPos, float speed)
    {
        if (moving)
        {
            Debug.Log("GridTransform failed because it was still moving.");
            return false; // finish move before starting next move
        }

        Terrain targetCell = grid.CellToTerrain(targetPos);

        if (targetCell != null) // tile must exist
        {
            GridCollider gc = GetComponent<GridCollider>();

            // solid objects cannot go to non-walkable tiles
            if (!targetCell.GetIsWalkable() && gc != null) return false;

            // check for collision
            if (gc != null)
                if (gc.CheckForCollision(targetPos) != GridCollider.CollisionType.None)
                {
                    Debug.Log("GridTransform failed because there was something in the way.");
                    return false;
                }

            Terrain startCell = grid.CellToTerrain(gridPosition);

            // Update cell residents
            startCell?.RemoveResident(this);
            targetCell.AddResident(this);

            Vector3 oldPosition = transform.position;
            Vector3 newPosition = targetCell.GetCellPosition() + offset;
            //Debug.Log("The oldPosition is " + oldPosition);
            //Debug.Log("The newPosition is " + newPosition);
            //Debug.Log("offset is: " + offset);
            StartCoroutine(Move(oldPosition, newPosition, speed));
            gridPosition = targetPos;
            onMoveSuccess?.Invoke(this, Vector3Int.RoundToInt(oldPosition), Vector3Int.RoundToInt(newPosition));

            return true;
        }


        Debug.Log("GridTransform failed because there was no terrain to move onto.");
        return false;
    }

    #endregion

    // Coroutine
    private IEnumerator Move(Vector3 oldPos, Vector3 newPos, float speed)
    {
        float elapsedTime = 0;
        moving = true;
        onMoveStart?.Invoke();

        while (elapsedTime < speed)
        {
            transform.position = Vector3.Lerp(oldPos, newPos, (elapsedTime / speed));
            elapsedTime += Time.deltaTime;
            onMoveDurring?.Invoke();
            // wait for next frame to move again
            yield return null;
        }

        // make sure to end at correct position
        moving = false;
        transform.position = newPos;
        onMoveEnd?.Invoke();
        yield return null;
    }

    public void Build()
    {
        if (grid == null)
            grid = FindObjectOfType<TerrainGrid>();

        //CalculateOffset();
        ForceSetGridPosition(gridPosition);
    }

    private void CalculateOffset()
    {
        // offsets the object so it looks like it is on the ground
        Renderer renderer = GetComponentInChildren<Renderer>();
        offset = new Vector3(0, renderer.bounds.size.y * 0.5f, 0);

        // removes whitespace from 
        SpriteRenderer spriteRenderer = renderer as SpriteRenderer;
        if (spriteRenderer != null)
        {
            Sprite sprite = spriteRenderer.sprite;

            float lowest = FindLowestPixelRatio(sprite);

            offset.y -= renderer.bounds.size.y * lowest;
        }
    }

    /// <summary>
    /// This finds the lowest non-transparent pixel and returns the ratio with the full size.
    /// </summary>
    /// <param name="sprite">Sprite to check.</param>
    /// <returns></returns>
    private float FindLowestPixelRatio(Sprite sprite)
    {
        if (sprite == null) return 0; // no sprite to read
        if (!sprite.texture.isReadable)
        {
            Debug.LogWarning("Sprite \'" + sprite.name + "\' on GameObject \'" + gameObject.name + "\' is not readable.");
            return 0; // don't read if not readable
        }

        Vector2 size = sprite.textureRect.size;

        // find first pixel in y axis from the bottom
        for (int i = Mathf.RoundToInt(size.y); i >= 0; --i)
        {
            for (int j = 0; j < size.x; j++)
            {
                Color color = sprite.texture.GetPixel(j, i);

                if (color != Color.clear)
                    return 1 - (i / size.y);
            }
        }

        // found nothing (empty sprite)
        Debug.LogWarning("SpriteRenderer for \'" + gameObject.name + "\' has an empty Texture2D.");
        return 1;
    }
}
