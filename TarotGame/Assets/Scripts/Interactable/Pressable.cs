using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pressable : Interactable
{
    [SerializeField]
    private UnityEvent onPressed;

    public override bool Interact(Interactor interactor)
    {
        onPressed.Invoke();
        onInteractionSuccess?.Invoke(interactor, this);
        return true;
    }

    public override void OnStateChange()
    {
        // do nothing
    }
}
