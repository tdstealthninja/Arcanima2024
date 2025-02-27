using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullDataView : MonoBehaviour
{
    [SerializeField]
    private TextView idView;
    [SerializeField]
    private TextView levelNumberView;
    [SerializeField]
    private TextView loadTimeView;
    [SerializeField]
    private GraphView fpsView;
    [SerializeField]
    private ActionListView actionView;

    private IEncoder encoder = new JsonEncoder();

    private LevelDebugData levelData;
    public LevelDebugData LevelData
    {
        get { return levelData; }
        set
        {
            levelData = value;
            UpdateFullDataView();
        }
    }

    public void UpdateFullDataView()
    {
        idView.Text = levelData.id;
        levelNumberView.Text = levelData.level.ToString();
        loadTimeView.Text = levelData.loadTime.Seconds.ToString("0.00000"); // show to 5 decimals
        fpsView.GraphData = levelData.framerate;
        actionView.Actions = levelData.actions;
    }

    public void OnFileLoaded(string data)
    {
        LevelData = encoder.Decode<LevelDebugData>(data);
    }
}
