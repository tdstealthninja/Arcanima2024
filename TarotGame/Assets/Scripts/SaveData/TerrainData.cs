using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TerrainData 
{
    public int[] pos = new int[3];
    public string groundID;
    public string liquidID;

    public TerrainData(Terrain t)
    {
        // Set position
        Vector3Int cellPos = t.GetCellPosition();
        pos[0] = cellPos.x;
        pos[1] = cellPos.y;
        pos[2] = cellPos.z;

        Ground g = t.GetComponentInChildren<Ground>();
        groundID = g ? g.GetType().Name : "NONE";

        Liquid l = t.GetComponentInChildren<Liquid>();
        liquidID = l ? l.GetType().Name : "NONE";
    }

    /// <summary>
    /// Converts the TerrainData to a GameObject
    /// </summary>
    /// <returns>Terrain GameObject</returns>
    public GameObject ToObject()
    {
        Debug.Log("Loading Terrain Object");
        GameObject go = GameObject.Instantiate(Resources.Load("Terrain/Terrain") as GameObject);
        Terrain t = go.GetComponent<Terrain>();

        GameObject onTilePosition = new("On Tile Position");
        onTilePosition.transform.parent = go.transform;

        // Cell position
        Vector3Int cellPos = new(pos[0], pos[1], pos[2]);
        t.SetCellPostion(cellPos);

        // Ground
        if(groundID != "NONE")
        {
            Debug.Log("Started " + groundID);
            GameObject ground = GameObject.Instantiate(Resources.Load("Terrain/Ground/" + groundID) as GameObject);
            ground.transform.parent = go.transform;
            Debug.Log("Finished " + groundID);
        }

        // Liquid
        if (liquidID != "NONE")
        {
            Debug.Log("Started " + liquidID);
            GameObject liquid = GameObject.Instantiate(Resources.Load("Terrain/Liquid/" + liquidID) as GameObject);
            liquid.transform.parent = go.transform;
            Debug.Log("Finished " + liquidID);
        }

        Debug.Log("Finished Loading Terrain Object");

        return go;
    }
}
