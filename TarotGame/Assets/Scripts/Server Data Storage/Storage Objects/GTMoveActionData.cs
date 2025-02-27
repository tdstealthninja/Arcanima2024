using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GTMoveActionData : ActionData
{
    public string gridTransformName;
    private int[] startPostion;
    private int[] endPostion;

    public Vector3Int StartPosition
    {
        get
        {
            return new Vector3Int(startPostion[0], startPostion[1], startPostion[2]);
        }
        set
        {
            startPostion = new int[3];
            startPostion[0] = value.x;
            startPostion[1] = value.y;
            startPostion[2] = value.z;
        }
    }

    public Vector3Int EndPosition
    {
        get
        {
            return new Vector3Int(endPostion[0], endPostion[1], endPostion[2]);
        }
        set
        {
            endPostion = new int[3];
            endPostion[0] = value.x;
            endPostion[1] = value.y;
            endPostion[2] = value.z;
        }
    }

    public GTMoveActionData(string gridTransformName, Vector3Int startPosition, Vector3Int endPosition) : base("GT Move")
    {
        this.gridTransformName = gridTransformName;
        StartPosition = startPosition;
        EndPosition = endPosition;

        description = ToString();
    }

    public override string ToString()
    {
        return gridTransformName + "\n" + StartPosition.ToString() + " -> " + EndPosition.ToString();
    }
}
