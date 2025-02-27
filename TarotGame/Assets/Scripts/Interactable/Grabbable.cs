using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Grabbable : Interactable
{
    private bool grabbed = false;
    private GridTransform gridTransform;

    [SerializeField]
    private int id;

    public UnityEvent onGrabStart;
    public UnityEvent onGrabEnd;


    private void Start()
    {
        gridTransform = GetComponent<GridTransform>();
    }

    public override bool Interact(Interactor interactor)
    {
        if (!gameObject.activeInHierarchy) return false;

        if (grabbed)
        {
            // Put Down
            grabbed = false;
            Vector3Int pos = transform.parent.GetComponent<GridTransform>().GetGridPosition();
            transform.parent = null;
            gridTransform.SetGridPosition(pos);
            onGrabEnd?.Invoke();
        }
        else
        {
            // Pick Up
            Transform grabPoint = interactor.transform;
            grabbed = true;
            // remove from terrain resident list (no longer a resident)
            GridTransform.GetTerrainGrid().CellToTerrain(gridTransform.GetGridPosition()).RemoveResident(gridTransform);
            transform.parent = grabPoint;
            transform.localPosition = new Vector3(0, 1, 0) * 0.35f;
            onGrabStart?.Invoke();
        }

        onInteractionSuccess?.Invoke(interactor, this);
        return true;
    }

    public int GetID() => id;

    public override void OnStateChange()
    {
        // do nothing
    }
}
