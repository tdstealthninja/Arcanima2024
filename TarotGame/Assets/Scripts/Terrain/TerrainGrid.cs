using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

// this runs before everything else
[DefaultExecutionOrder(-1)]
public class TerrainGrid : MonoBehaviour
{
    private GridLayout grid;
    private Terrain[] terrains;

    // Start is called before the first frame update
    void Awake()
    {
        UpdateTerrainList();

        GridTransform.SetTerrainGrid(this);
        GridCollider.SetGrid(this);

        BuildGrid();
    }

    public Terrain[] GetTerrains()
    {
        UpdateTerrainList();
        return terrains;
    }

    private void CombineLiquids()
    {
        Debug.LogError("TerrainGrid.CombineLiquids() is not implemented.");
    }

    private void CalculateGroundDirections()
    {
        Debug.LogError("TerrainGrid.CalculateGroundDirections() is not implemented.");
    }

    public void UpdateTerrainList() => terrains = GetComponentsInChildren<Terrain>();

    public Terrain CellToTerrain(Vector3Int targetCell)
    {
        Terrain cell = null;

        // Prevents error in editor
        UpdateTerrainList();

        //Debug.Log(terrains.Length);

        foreach (Terrain terrain in terrains)
        {
            if (terrain.GetCellPosition() == targetCell)
            {
                cell = terrain;
                break;
            }
        }

        return cell;
    }

    public void BuildGrid()
    {
        grid = GetComponent<Grid>();

        Debug.Log("Grid: " + grid);

        // Place objects in grid
        UpdateTerrainList();
        for (int i = 0; i < terrains.Length; i++)
        {
            Terrain terrain = terrains[i];
            Debug.Log("Terrain: " + terrain);
            Vector3 cellPos = grid.CellToLocal(terrain.GetCellPosition());
            terrain.transform.localPosition = cellPos;
            terrain.PlaceComponents();
        }

        GridTransform[] gridTransforms = FindObjectsOfType<GridTransform>();
        foreach (GridTransform gridTransform in gridTransforms)
        {
            Debug.Log("Building " + gridTransform);
            gridTransform.Build();
        }
    }

    public void ClearLevel()
    {
        UpdateTerrainList();

        foreach (Terrain terrain in terrains)
        {
            List<GridTransform> residents = terrain.GetResidents();
            List<GameObject> decorations = terrain.GetDecorations();
            //indexing backwards for performance
            for (int i = residents.Count - 1; i >= 0 ; i--)
            {
                Destroy(residents[i].gameObject);
            }

            for (int i = decorations.Count - 1; i >= 0 ; i--)
            {
                Destroy(decorations[i].gameObject);
            }

            Destroy(terrain.gameObject);
        }

        terrains = null;
    }

    public Vector3Int GetClosestCell(Vector3 position) => grid.WorldToCell(position);
    public Vector3 CellToLocal(Vector3Int gridPos) => grid.CellToLocal(gridPos);
    public Vector3 CellToWorld(Vector3Int gridPos) => grid.CellToWorld(gridPos);

#if UNITY_EDITOR
    [CustomEditor(typeof(TerrainGrid))]
    public class TerrainGridEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            TerrainGrid grid = target as TerrainGrid;

            if (GUILayout.Button("Build Grid"))
            {
                grid.BuildGrid();
            }
        }
    }
#endif
}
