using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLogger : MonoBehaviour, IDataLogger
{
    private bool started = false;

    private float time;

    public void Log()
    {
        if (!started)
            time = Time.realtimeSinceStartup; // starting, set offset
        else
            time = Time.realtimeSinceStartup - time; // ending, store time since offset

        started = !started; // start or stop
    }

    public IData Return()
    {
        return new TimeData(time);
    }
}
