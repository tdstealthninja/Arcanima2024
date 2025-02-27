using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToggleShadow : MonoBehaviour
{
    private Shadow shadow;
    private GridTransform shadowGridTransform;
    private GridTransform playerGridTransform;

    private bool shadowEnabled = false;

    public UnityEvent onShadowToggle;

    public void Init()
    {
        shadow = FindObjectOfType<Shadow>();
        shadowGridTransform = shadow.GetComponent<GridTransform>();
        playerGridTransform = FindObjectOfType<PlayerMovement>().GetComponent<GridTransform>(); // Player movement requires GridTransform
        shadow.gameObject.SetActive(shadowEnabled);
        shadow.enabled = shadowEnabled;
    }

    public void Toggle()
    {
        if (!playerGridTransform.IsMoving)
        {
            shadowEnabled = !shadowEnabled; // switch bool state
            shadow.DropHeldItem();
            shadow.gameObject.SetActive(shadowEnabled);
            shadow.enabled = shadowEnabled;
            onShadowToggle?.Invoke();
            shadowGridTransform.ForceSetGridPosition(playerGridTransform.GetGridPosition());
            shadowGridTransform.transform.position = playerGridTransform.transform.position;
            playerGridTransform.onMoveEnd.RemoveListener(Toggle);
        }
        else
        {
            playerGridTransform.onMoveEnd.RemoveListener(Toggle);
            playerGridTransform.onMoveEnd.AddListener(Toggle);
        }
    }
}
