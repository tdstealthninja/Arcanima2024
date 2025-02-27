using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Interactor))]
public class PlayerInteraction : MonoBehaviour
{
    private Interactor interactor;

    private void Awake()
    {
        interactor = GetComponent<Interactor>();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
            interactor.Interact();
    }
}
