using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Placement : MonoBehaviour
{
    public GameObject currentSelected;

    protected TerrainGrid grid;

    private GameObject terrainPrefab;

    private SaveGTDataManagerV2 saveManager;

    // Delegate function
    private delegate void PlaceObject(Vector3Int gridPos, Placement placement);
    private PlaceObject placeObject;

    private void Start()
    {
        grid = FindObjectOfType<TerrainGrid>();
        saveManager = FindObjectOfType<SaveGTDataManagerV2>();
        terrainPrefab = saveManager.objectPrefabList.debugObjs[0];
        Debug.Log(terrainPrefab);
    }

    public void SetCurrent(GameObject current)
    {
        currentSelected = current;

        // This section of code decides what type of placement function it should use

        //      This uses delegates which allows it to select the function it should
        //      use without having to check everytime it is called

        if (currentSelected == null)
        {
            placeObject = null; // nothing to place, don't need a delegate
            return;
        }

        Ground ground = currentSelected.GetComponent<Ground>();
        if (ground != null)
        {
            placeObject = placeGround; // use the placeGround delegate function
            return;
        }

        GridTransform gridTransform = currentSelected.GetComponent<GridTransform>();
        if (gridTransform != null)
        {
            placeObject = placeGridTransform; // use the placeGridTransform delegate function
            return;
        }

        if (ground == null && gridTransform == null)
        {
            placeObject = placeDecoration; // if both above checks fail, use the placeDecoration delegate function
            return;
        }
    }

    // Place just invokes the delegate if it is set
    public void Place(Vector3Int gridPos)
    {
        if (grid == null)
            grid = FindObjectOfType<TerrainGrid>();

        placeObject?.Invoke(gridPos, this);
    }

    public void Remove(Vector3Int gridPos)
    {
        if (grid == null)
            grid = FindObjectOfType<TerrainGrid>();

        Terrain target = grid.CellToTerrain(gridPos);
        List<GridTransform> gtTargets = target.GetResidents();
        List<GameObject> dTargets = target.GetDecorations();


        if (target != null)
        {
            if (gtTargets.Count > 0)
            {
                Destroy(gtTargets[0].gameObject);
                target.RemoveResident(gtTargets.ElementAt<GridTransform>(0));
            }

            else if (dTargets.Count > 0)
            {
                Destroy(dTargets[0].gameObject);
                target.RemoveDecoration(dTargets.ElementAt<GameObject>(0));
            }

            else
            {
                Destroy(target.gameObject);

                grid.UpdateTerrainList();
            }            
        }
    }


    // Place terrain if neccissary
    protected Terrain PlaceTerrain(Vector3Int gridPos)
    {
        Vector3 targetPos = grid.CellToWorld(gridPos); // Get correct world position for the grid

        // create terrain
        GameObject go = Instantiate(terrainPrefab, targetPos, Quaternion.identity);
        go.transform.SetParent(grid.transform);

        // set the position
        Terrain terrain = go.GetComponent<Terrain>();
        terrain.SetCellPostion(gridPos);

        // update the grid because there is a new terrain piece
        grid.UpdateTerrainList();

        return terrain;
    }

    // delegate for placing ground
    private PlaceObject placeGround = (Vector3Int gridPos, Placement placement) =>
    {
        Terrain targetTerrain = placement.grid.CellToTerrain(gridPos); // Get terrain

        if (targetTerrain == null)
        {
            targetTerrain = placement.PlaceTerrain(gridPos);
        }

        Ground[] testGrounds = targetTerrain.GetComponentsInChildren<Ground>();

        if (testGrounds.Length < 1)
        {
            GameObject go = Instantiate(placement.currentSelected, targetTerrain.transform);

            targetTerrain.PlaceComponents();
        }

        
    };

    // delegate for placing grid transforms
    private PlaceObject placeGridTransform = (Vector3Int gridPos, Placement placement) =>
    {
        GameObject go = Instantiate(placement.currentSelected, gridPos, Quaternion.identity);
        GridTransform gridTransform = go.GetComponent<GridTransform>();
        if (gridTransform != null)
        {
            TerrainGrid grid = FindObjectOfType<TerrainGrid>();
            Terrain t = grid.CellToTerrain(gridPos);
            if(t != null && t.GetResidents().Count != 0)
            {
                Destroy(go);
                return;
            }
            bool success = gridTransform.SetGridPosition(gridPos, 0);
            if (success == false) Destroy(go); // delete gameobject if it cant be placed
        }
    };

    // delegate for placing decorations
    private PlaceObject placeDecoration = (Vector3Int gridPos, Placement placement) =>
    {
        
        RaycastHit hit;
        Camera camera = Camera.main;
        Ray ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        bool hitSuccess = Physics.Raycast(ray, out hit);
        Debug.Log(hitSuccess);
        Debug.DrawRay(camera.transform.position, camera.transform.TransformDirection(Vector3.forward));
        if (hitSuccess)
        {
            GameObject go = Instantiate(placement.currentSelected, gridPos, Quaternion.identity);
            Terrain t = hit.collider.gameObject.transform.parent.GetComponent<Terrain>();
            if (t != null)
            {
                //Check if there are any GridTrasnforms on Terrain, if not place decoration. 
                //This check is so anything with an interactable object won't have a decoration placed on top of the object.
                if (t.GetResidents().Count <= 0)
                {
                    go.transform.position = new Vector3(hit.point.x, t.GetOnTilePosition().y, hit.point.z);
                    bool positionOccupied = false;
                    foreach (GameObject decoration in t.GetDecorations())
                    {
                        if (go.transform.position == decoration.transform.position)
                        {
                            positionOccupied = true;
                        }
                    }
                    if (!positionOccupied)
                    {
                        t.AddDecoration(go);
                    }
                    else
                    {
                        Destroy(go);
                        return;
                    }  
                }
                else
                {
                    Destroy(go);
                    return;
                }
            }
            else
            {
                Destroy(go);
                return;
            }
        }
    };


}
