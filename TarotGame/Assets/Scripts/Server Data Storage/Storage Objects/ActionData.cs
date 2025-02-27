using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ActionData : IData
{
    /// <summary>
    /// The offset is used as the start time for this set of actions that is being recorded.
    /// </summary>
    private static float timeOffset;
    public string actionName;
    public float actionTime;
    public string description = "";

    public ActionData(string actionName)
    {
        this.actionName = actionName;
        actionTime = Time.realtimeSinceStartup - timeOffset;
        Debug.Log(actionTime);
    }

    /// <summary>
    /// Sets the timeOffset to be now.
    /// </summary>
    public static void SetOffsetToNow()
    {
        timeOffset = Time.realtimeSinceStartup;
    }

    public override string ToString()
    {
        return description;
    }
}
