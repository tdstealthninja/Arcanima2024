using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    [Flags]
    public enum GroundDirection
    {
        None = 0,
        North = 1,
        South = 2,
        East = 4,
        West = 8
    }

    private GroundDirection groundDirection;
    private MeshRenderer meshRenderer;
    private List<Mesh> meshList;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    /// <summary>
    /// Sets the mesh that the terrain uses based on direction.
    /// </summary>
    /// <param name="groundDirection"></param>
    /// <returns></returns>
    public bool SetDirection(GroundDirection groundDirection)
    {
        this.groundDirection = groundDirection;
        throw new System.NotImplementedException();
        return false;
    }
}
