using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.Mathematics;
using UnityEngine;

public class Terrain : MonoBehaviour
{
    [SerializeField]
    private Vector3Int cellPosition;
    [SerializeField]
    private List<GridTransform> residents = new List<GridTransform>();
    [SerializeField]
    private List<GameObject> decorations = new List<GameObject>();
    [SerializeField]
    private bool isWalkable = true;

    private Ground ground;
    private Ground.GroundDirection groundDirection;
    private Liquid liquid;
    private Transform onTileSpace;

    // Start is called before the first frame update
    void Awake()
    {
        ground = GetComponentInChildren<Ground>();
        liquid = GetComponentInChildren<Liquid>();
        onTileSpace = transform.Find("On Tile Position");
        
    }

    #region Getters and Setters

    public List<GridTransform> GetResidents() => residents;

    public List<GameObject> GetDecorations() => decorations;

    public void AddResident(GridTransform r) => residents.Add(r);

    public void RemoveResident(GridTransform r) => residents.Remove(r);

    public void AddDecoration(GameObject d) => decorations.Add(d);

    public void RemoveDecoration(GameObject d) => decorations.Remove(d);

    public void SetCellPostion(Vector3Int gridPosition) => cellPosition = gridPosition;

    public Vector3Int GetCellPosition() => cellPosition;

    public bool GetIsWalkable() => isWalkable;

    public void SetWalkable(bool walkable) => isWalkable = walkable;

    public Vector3 GetOnTilePosition() => onTileSpace.position;

    #endregion

    // Places the ground and liquid at correct heights
    // Called by BuildGrid
    public void PlaceComponents()
    {
        ground = GetComponentInChildren<Ground>();
        liquid = GetComponentInChildren<Liquid>();
        onTileSpace = transform.Find("On Tile Position");

        if (ground != null)
            ground.transform.localPosition = Vector3.zero;
        if (liquid != null)
            liquid.transform.localPosition = new Vector3(0, -0.25f, 0);

        onTileSpace.localPosition = new Vector3(0, ground.GetComponentInChildren<Renderer>().bounds.size.y * 0.5f, 0);
    }
}
