using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GridTransformData 
{
    public string name;
    public int[] pos = new int[3];

    public GridTransformData(GridTransform gt)
    {
        // Name of the object
        name = gt.gameObject.name;
        name = name.Replace("(Clone)", "");

        // Position
        Vector3Int cellPos = gt.GetGridPosition();
        pos[0] = cellPos.x;
        pos[1] = cellPos.y;
        pos[2] = cellPos.z;
    }

    public GameObject ToObject()
    {
        Debug.Log("Starting " + name);
        GameObject toMake = Resources.Load("Object/" + name) as GameObject;
        if (toMake == null) Debug.LogError("Object/" + name + " was not found");
        GameObject go = GameObject.Instantiate(toMake);
        GridTransform gt = go.GetComponent<GridTransform>();

        Vector3Int cellPos = new(pos[0], pos[1], pos[2]);
        gt.ForceSetGridPosition(cellPos);
        gt.Build();

        Debug.Log("Finished " + name);

        return go;
    }
}