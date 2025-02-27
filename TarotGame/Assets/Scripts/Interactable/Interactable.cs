using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Interactable : MonoBehaviour
{
    public UnityEvent<Interactor, Interactable> onInteractionSuccess;

    private void Start()
    {
        int interactableLayer = LayerMask.NameToLayer("Interactable");
        if (gameObject.layer != interactableLayer)
        {
            gameObject.layer = interactableLayer;
            Debug.LogWarning("Interactable objects must be on the Interactable layer.");
        }
    }

    public abstract bool Interact(Interactor interactor);

    public abstract void OnStateChange();
}
