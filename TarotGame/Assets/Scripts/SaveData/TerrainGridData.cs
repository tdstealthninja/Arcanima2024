using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TerrainGridData
{
    public TerrainData[] terrainData;
    public GridTransformData[] gridTransformData;
    public DecorationsData[] decorationsData;

    public TerrainGridData(TerrainGrid tg)
    {
        // Set up lists
        List<TerrainData> terrainDataList = new();
        List<GridTransformData> gridTransformDataList = new();
        List<DecorationsData> decorationsDataList = new();

        // go through all terrains
        foreach (Terrain t in tg.GetTerrains())
        {
            // get residents of t
            List<GridTransform> residents = t.GetResidents();

            // get decorations of t
            List<GameObject> decorations = t.GetDecorations();

            // go through all residents
            foreach (GridTransform resident in residents)
                gridTransformDataList.Add(new GridTransformData(resident));

            // go through all decorations
            foreach (GameObject decoration in decorations)
                decorationsDataList.Add(new DecorationsData(decoration));

            terrainDataList.Add(new TerrainData(t));
        }

        // save the data
        terrainData = terrainDataList.ToArray();
        gridTransformData = gridTransformDataList.ToArray();
        decorationsData = decorationsDataList.ToArray();
    }

    public GameObject ToObject()
    {
        GameObject go = GameObject.Instantiate(Resources.Load("Terrain/TerrainGrid") as GameObject);
        TerrainGrid tg = go.GetComponent<TerrainGrid>();

        Debug.Log("Loading Terrain");
        if (terrainData != null)
            foreach (TerrainData t in terrainData)
                t.ToObject().transform.parent = go.transform;

        tg.UpdateTerrainList();

        Debug.Log("Loading GridTransform");
        if (gridTransformData != null)
            foreach (GridTransformData gt in gridTransformData)
                gt.ToObject();

        Debug.Log("Loading Decorations");
        if (decorationsData != null)
            foreach (DecorationsData dc in decorationsData)
                dc.ToObject();

        Debug.Log("Building Grid");
        tg.BuildGrid();

        return go;
    }
}
