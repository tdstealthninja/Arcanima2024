using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Replace Placeholders before doing anything else
[DefaultExecutionOrder(-1)]
public class PlaceholderData : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;
    private GridTransform gridTransform;

    private void Awake()
    {
        gridTransform = GetComponent<GridTransform>();
        FindObjectOfType<PlayLoadedLevelV2>().onLoadEnded.RemoveListener(OnGameStart);
        FindObjectOfType<PlayLoadedLevelV2>().onLoadEnded.AddListener(OnGameStart);
    }

    private void OnGameStart()
    {
        GameObject go = Instantiate(prefab);
        GridTransform gt = go.GetComponent<GridTransform>();
        gt.ForceSetGridPosition(gridTransform.GetGridPosition());
        Destroy(gameObject);
    }
}
