using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionLogger : MonoBehaviour
{
    private List<ActionData> actions = new();

    public void LogAction(string name)
    {
        actions.Add(new ActionData(name));
    }

    public void LogGridMove(GridTransform gt, Vector3Int start, Vector3Int end)
    {
        actions.Add(new GTMoveActionData(gt.gameObject.name, start, end));
    }

    public void LogInteraction(Interactor interactor, Interactable interacted)
    {
        actions.Add(new InteractActionData(interactor.gameObject.name, interacted.gameObject.name));
    }

    public ActionData[] Return() => actions.ToArray();
}
