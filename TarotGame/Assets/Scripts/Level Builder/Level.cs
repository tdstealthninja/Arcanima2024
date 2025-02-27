using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public int id;
    public Vector3 location;

    public LevelData(int newId, Vector3 newLoc)
    {
        id = newId;
        location = newLoc;
    }

    int getID() {return id;}
    Vector3 getLoc() {return location;}

    public override bool Equals(object data)
    {
        if (data == null)
            return false;
        if (GetType() != data.GetType())
            return false;
        
        var leveldata = (LevelData)data;

        return (id == leveldata.id) && (location == leveldata.location);
    }

    public override int GetHashCode()
    {
        return ToString().GetHashCode();
    }

    public static bool operator ==(LevelData dataLeft, LevelData dataRight)
{
    if (dataLeft.Equals(dataRight))
        return true;
    else
        return false;
}

public static bool operator !=(LevelData dataLeft, LevelData dataRight)
{
    if (dataLeft.Equals(dataRight))
        return false;
    else
        return true;
}
}


[System.Serializable]
public class Level : ScriptableObject
{
    [SerializeField]
     private int version = 2;
     ////public List<int> levelObjectIds = new List<int>();
    //  public List<int> levelTerrainIds = new List<int>();
    //  public List<int> levelGTIds = new List<int>();
    //  public List<int> levelDecorationIds = new List<int>();
    //  //public List<Vector3> levelObjectLocations = new List<Vector3>();

    //  public List<Vector3> levelTerrainLocations = new List<Vector3>();
    //  public List<Vector3> levelGTLocations = new List<Vector3>();
    //  public List<Vector3> levelDecorationLocations = new List<Vector3>();


     public List<LevelData> terrainLevelData = new List<LevelData>();
     public List<LevelData> gtLevelData = new List<LevelData>();
     public List<LevelData> decorationLevelData = new List<LevelData>();
     public List<LevelData> npcLevelData = new List<LevelData>();

     public void InitLevel(LevelObjects prefabList) 
     {
        TerrainGrid tg = FindObjectOfType<TerrainGrid>();
        Terrain[] terrainArray = tg.GetTerrains();
        List<GridTransform> gtList = new List<GridTransform>();
        List<GameObject> decorations = new List<GameObject>();
        List<GridTransform> npcs = new List<GridTransform>();

        foreach (Terrain terrain in terrainArray)
        {
            //gtList.AddRange(terrain.GetResidents());
            foreach (GridTransform gridTransform in terrain.GetResidents())
            {
                if (gridTransform.gameObject.GetComponent<Conversable>() != null)
                {
                    npcs.Add(gridTransform);
                }
                else
                    gtList.Add(gridTransform);
            }
            decorations.AddRange(terrain.GetDecorations());
            foreach (GameObject prefab in prefabList.terrainPrefabs)
            {
                if (prefab.GetComponentInChildren<Grass>() != null && terrain.GetComponentInChildren<Grass>() != null) 
                {
                    int objectId = prefabList.terrainPrefabs.IndexOf(prefab);
                    LevelData grass = new LevelData(objectId, terrain.GetCellPosition());
                    terrainLevelData.Add(grass);
                    // levelTerrainIds.Add(objectId);
                    // levelTerrainLocations.Add(terrain.GetCellPosition());
                }

                else if (prefab.GetComponentInChildren<Sand>() != null && terrain.GetComponentInChildren<Sand>() != null) 
                {
                    int objectId = prefabList.terrainPrefabs.IndexOf(prefab);
                    LevelData sand = new LevelData(objectId, terrain.GetCellPosition());
                    terrainLevelData.Add(sand);
                    // levelTerrainIds.Add(objectId);
                    // levelTerrainLocations.Add(terrain.GetCellPosition());
                }

                else if (prefab.GetComponentInChildren<Tiles>() != null && terrain.GetComponentInChildren<Tiles>() != null)
                {
                    int objectId = prefabList.terrainPrefabs.IndexOf(prefab);
                    LevelData tiles = new LevelData(objectId, terrain.GetCellPosition());
                    terrainLevelData.Add(tiles);
                    // levelTerrainIds.Add(objectId);
                    // levelTerrainLocations.Add(terrain.GetCellPosition());
                }

                else if (prefab.GetComponentInChildren<Water>() != null && terrain.GetComponentInChildren<Water>() != null) 
                {
                    int objectId = prefabList.terrainPrefabs.IndexOf(prefab);
                    LevelData water = new LevelData(objectId, terrain.GetCellPosition());
                    terrainLevelData.Add(water);
                    // levelTerrainIds.Add(objectId);
                    // levelTerrainLocations.Add(terrain.GetCellPosition());
                }                
            }
            
        }

        foreach (GridTransform gridTransform in gtList)
        {
            foreach (GameObject prefab in prefabList.gridTransformPrefabs)
            {
                if (gridTransform.gameObject.name == string.Format("{0}(Clone)", prefab.name)) 
                {
                    int objectId = prefabList.gridTransformPrefabs.IndexOf(prefab);
                    LevelData gtObject = new LevelData(objectId, gridTransform.GetGridPosition());
                    gtLevelData.Add(gtObject);
                    // levelGTIds.Add(prefabList.gridTransformPrefabs.IndexOf(prefab));
                    // levelGTLocations.Add(gridTransform.GetGridPosition());
                }
                
            }
        }
        
        foreach (GameObject decoration in decorations)
        {
            foreach (GameObject prefab in prefabList.decorationPrefabs)
            {
                if (decoration.name == string.Format("{0}(Clone)", prefab.name)) 
                {
                    int objectId = prefabList.decorationPrefabs.IndexOf(prefab);
                    LevelData decorationObject = new LevelData(objectId, decoration.transform.position);
                    bool decorationExists = false;
                    foreach (LevelData levelData in decorationLevelData)
                    {
                        if (decorationObject == levelData)
                            decorationExists = true;
                    }
                    if (!decorationExists)
                        decorationLevelData.Add(decorationObject);
                    // levelDecorationIds.Add(prefabList.decorationPrefabs.IndexOf(prefab));
                    // levelDecorationLocations.Add(decoration.transform.position);
                }
            }
        }

        foreach (GridTransform npc in npcs)
        {
            foreach (GameObject prefab in prefabList.NpcPrefabs)
            {
                if (npc.gameObject.name == string.Format("{0}(Clone)", prefab.name))
                {
                    int objectId = prefabList.NpcPrefabs.IndexOf(prefab);
                    LevelData npcObject = new LevelData(objectId, npc.GetGridPosition());
                    npcLevelData.Add(npcObject);
                    // levelGTIds.Add(prefabList.gridTransformPrefabs.IndexOf(prefab));
                    // levelGTLocations.Add(gridTransform.GetGridPosition());
                }
            }
        }


    }
}
