using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridTransform))]
public class GridCollider : MonoBehaviour
{
    private GridTransform gridTransform;
    private static TerrainGrid grid;
    private Vector3Int pos; // used for updating old position

    [Flags]
    public enum CollisionFlags
    {
        None = 0,
        Player = 1,
        Shadow = 2,
        Pushable = 4,
        Interactable = 8
    }

    public enum CollisionType
    {
        None = 0,
        Pushable,
        Interactable,
        Blocked
    }

    [SerializeField]
    private CollisionFlags iCollideWith = CollisionFlags.None;
    [SerializeField, Tooltip("Flag of this object")]
    private CollisionFlags collidesWithMe = CollisionFlags.None;
    [SerializeField]
    private bool locked = false;
    [SerializeField]
    private bool mirror = false;



    public bool isLocked
    {
        get { return locked; }
        private set { locked = value; }
    }

    public bool isMirror
    {
        get { return mirror; }
        private set { mirror = value; }
    }

    public CollisionFlags CollidesWithMe { get { return collidesWithMe; } set { collidesWithMe = value; } }
    public CollisionFlags ICollideWith { get { return iCollideWith; } set { iCollideWith = value; } }

    private void Awake()
    {
        gridTransform = GetComponent<GridTransform>();
    }

    public static void SetGrid(TerrainGrid tg) => grid = tg;

    /// <summary>
    /// Checks if there is a collider at gridPos.
    /// </summary>
    /// <param name="gridPos"></param>
    /// <returns>True if there is an object.</returns>
    public CollisionType CheckForCollision(Vector3Int gridPos)
    {
        Terrain t = grid.CellToTerrain(gridPos);

        if (t != null)
            foreach (GridTransform r in t.GetResidents())
            {
                if (r != null && r != gridTransform)
                {
                    GridCollider gc = r.GetComponent<GridCollider>();
                    if (gc != null && gc.gameObject.activeInHierarchy)
                    {
                        if ((iCollideWith & gc.collidesWithMe) != CollisionFlags.None)
                        {
                            if (gc.GetComponent<Interactable>())
                                return CollisionType.Interactable;
                            else if (gc.isLocked)
                                return CollisionType.Blocked;
                            else
                                return CollisionType.Pushable;
                        }
                    }
                }
            }
        else
            return CollisionType.Blocked; // no tile to move onto (blocked from moving)

        return CollisionType.None;
    }
}
