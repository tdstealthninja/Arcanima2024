using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GraphView : MonoBehaviour
{
    [SerializeField]
    private GameObject graphElementPrefab;
    [SerializeField]
    private Transform content;
    [SerializeField]
    private TextMeshProUGUI topLabel;
    [SerializeField]
    private TextMeshProUGUI middleLabel;
    [SerializeField]
    private TextMeshProUGUI bottomLabel;
    [SerializeField]
    private GraphData graphData;

    [SerializeField]
    private float min;
    private float mid;
    [SerializeField]
    private float max;

    public GraphData GraphData
    {
        get { return graphData; }
        set
        {
            graphData = value;
            UpdateGraphView();
        }
    }

    private void UpdateGraphView()
    {
        // find mid point
        mid = Mathf.Lerp(max, min, 0.5f);
        mid = Mathf.Round(mid);

        // set text
        topLabel.text = max.ToString();
        middleLabel.text = mid.ToString();
        bottomLabel.text = min.ToString();

        foreach(Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }

        // generate elements
        for (int i = 0; i < graphData.Count; i++)
        {
            float v = graphData[i];

            GameObject go = Instantiate(graphElementPrefab, content);
            go.GetComponent<GraphViewElement>().SetValue(min, max, v);
        }
    }
}
