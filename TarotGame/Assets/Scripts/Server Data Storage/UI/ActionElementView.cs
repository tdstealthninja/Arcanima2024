using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActionElementView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI label;
    [SerializeField]
    private TextMeshProUGUI time;
    [SerializeField]
    private TextMeshProUGUI description;

    private ActionData actionData;
    public ActionData ActionData
    {
        get { return actionData; }
        set
        {
            actionData = value;
            UpdateActionElementView();
        }
    }

    public void UpdateActionElementView()
    {
        label.text = actionData.actionName;
        time.text = actionData.actionTime.ToString("0.00"); // only show 2 decimals

        if (actionData is GTMoveActionData)
            description.text = (actionData as GTMoveActionData).ToString();
        else if (actionData is InteractActionData)
            description.text = (actionData as InteractActionData).ToString();
        else
            description.text = actionData.ToString();
    }
}
