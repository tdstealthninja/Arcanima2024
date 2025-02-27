using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

[Serializable]
public class DecorationsData
{
    public string name;
    public float[] pos = new float[3];

    public DecorationsData(GameObject go)
    {
        // Name of the object
        name = go.gameObject.name;
        name = name.Replace("(Clone)", "");

        // Position
        Vector3 currentPos = go.transform.position;
        pos[0] = currentPos.x;
        pos[1] = currentPos.y;
        pos[2] = currentPos.z;
    }

    public GameObject ToObject()
    {
        Debug.Log("Starting decoration " + name);
        GameObject go = GameObject.Instantiate(Resources.Load("Decorations/" + name) as GameObject);

        Vector3 loadedPos = new(pos[0], pos[1], pos[2]);
        go.transform.position = loadedPos;

        Debug.Log("Finished decoration " + name);

        return go;
    }
}
