using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSLogger : MonoBehaviour, IDataLogger
{
    [SerializeField]
    private float interval = 0.5f;

    private float timeSinceLastLog = 0;
    List<float> logs = new();

    // Update is called once per frame
    void Update()
    {
        timeSinceLastLog += Time.deltaTime;

        if (timeSinceLastLog >= interval)
        {
            Log();
            timeSinceLastLog = 0;
        }
    }

    public void Log()
    {
        float fps = 1.0f / Time.deltaTime;
        fps = Mathf.Round(fps * 10) / 10; // rounds to one decimal
        logs.Add(fps);
    }

    public IData Return()
    {
        return new GraphData(logs.ToArray());
    }
}
