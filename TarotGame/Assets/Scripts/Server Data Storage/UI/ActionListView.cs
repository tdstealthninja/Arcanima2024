using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionListView : MonoBehaviour
{
    [SerializeField]
    private Transform content;
    [SerializeField]
    private GameObject elementPrefab;

    private ActionData[] actions;
    public ActionData[] Actions
    {
        get { return actions; }
        set
        {
            actions = value;
            UpdateActionListView();
        }
    }

    public void UpdateActionListView()
    {
        foreach(Transform element in content)
        {
            Destroy(element.gameObject);
        }

        foreach(ActionData action in actions)
        {
            GameObject go = Instantiate(elementPrefab, content);
            go.GetComponent<ActionElementView>().ActionData = action;
        }
    }
}
