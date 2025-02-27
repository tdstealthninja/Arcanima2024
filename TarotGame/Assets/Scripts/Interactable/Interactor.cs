using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactor : MonoBehaviour
{
    [SerializeField]
    private float range;
    private Interactable closestInteractable;

    // Make sure the player cannot spam interactions
    private float interactionCooldown = 0.1f;
    private bool canInteract = true;

    private int interactableMask;

    public UnityEvent onInteract;

    private void Start()
    {
        interactableMask = -LayerMask.NameToLayer("Interactable"); // only get objects on Interactable layer
    }

    private void Update()
    {
        closestInteractable = ClosestInteractable();
    }

    public void Interact()
    {
        if (canInteract)
        {
            // Interact if closestInteractable is not null
            closestInteractable?.Interact(this);
            onInteract?.Invoke();

            canInteract = false;
            StartCoroutine(InteractCooldown());
        }
    }

    private Interactable ClosestInteractable()
    {
        Interactable closestInteractable = this.closestInteractable;
        float currentClosestDist = float.PositiveInfinity;
        Vector3 posToCheck;
        Vector3 currentPos = transform.position;

        if (closestInteractable != null)
        {
            posToCheck = closestInteractable.transform.position;
            Vector3 diff = currentPos - posToCheck;
            if (diff.magnitude < range)
                currentClosestDist = diff.magnitude;
            else
                closestInteractable = null;
        }

        RaycastHit[] hits = Physics.SphereCastAll(currentPos, range, Vector3.up, 0.01f, interactableMask);

        foreach (RaycastHit hit in hits)
        {
            posToCheck = hit.transform.position;
            Vector3 diff = currentPos - posToCheck;

            if (currentClosestDist > diff.magnitude)
            {
                currentClosestDist = diff.magnitude;
                closestInteractable = hit.transform.GetComponent<Interactable>();
            }
        }

        return closestInteractable;
    }

    private IEnumerator InteractCooldown()
    {
        yield return new WaitForSeconds(interactionCooldown); // waits interactionCooldown seconds
        canInteract = true;
    }
}
