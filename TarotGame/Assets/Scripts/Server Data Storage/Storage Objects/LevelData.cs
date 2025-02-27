using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelDebugData : IData
{
    public string id; // id of the user who is playing
    public int level; // the current level number
    public GraphData framerate; // graph that displays the framerate over the course of the level
    public TimeData loadTime; // how long it took to load the level
    public ActionData[] actions; // list of all of the actions that the player performed over the course of the level
}
