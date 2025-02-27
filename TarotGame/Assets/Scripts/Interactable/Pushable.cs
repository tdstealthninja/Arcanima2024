using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(GridTransform))]
public class Pushable : Interactable
{
    private GridTransform gridTransform;
    [SerializeField]
    private bool locked = false;

    public UnityEvent onPush;

    [SerializeField]
    private Sprite[] sprites;

    private void Awake()
    {
        gridTransform = GetComponent<GridTransform>();

        FindObjectOfType<StateChange>().onStateChange.RemoveListener(OnStateChange);
        FindObjectOfType<StateChange>().onStateChange.AddListener(OnStateChange);
    }

    public override bool Interact(Interactor interactor)
    {
        if (locked) return false; // do not push if locked

        Vector3Int pushDirection = Vector3Int.RoundToInt(transform.position - interactor.transform.position);

        bool success = gridTransform.SetGridPosition(gridTransform.GetGridPosition() + pushDirection);

        if (success)
            onInteractionSuccess?.Invoke(interactor, this);

        return success;
    }

    public override void OnStateChange()
    {
        locked = !locked;
        GetComponentInChildren<SpriteRenderer>().sprite = sprites[locked ? 1 : 0];
    }
}
