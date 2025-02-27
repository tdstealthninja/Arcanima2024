using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

public class Torch : Interactable
{
    public enum LightLevel
    {
        Extinguished = 0,
        Dim,
        Bright
    }

    public static List<Torch> torches = new();
    public static List<Torch> brightTorches = new();
    public static List<Torch> dimTorches = new();

    public UnityEvent onExtinguish;

    [SerializeField]
    public LightLevel lightLevel { get; private set; }
    [SerializeField]
    private Sprite[] sprites;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        torches.Add(this);
        UpdateVisuals();

        FindObjectOfType<StateChange>().onStateChange.RemoveListener(OnStateChange);
        FindObjectOfType<StateChange>().onStateChange.AddListener(OnStateChange);
    }

    private void OnDestroy()
    {
        brightTorches.Remove(this);
        dimTorches.Remove(this);
        torches.Remove(this);
    }

    public override bool Interact(Interactor interactor)
    {
        if(lightLevel == LightLevel.Extinguished)
        {
            lightLevel = LightLevel.Bright;
            brightTorches.Add(this);
            UpdateVisuals();
            onInteractionSuccess?.Invoke(interactor, this);
            return true;
        }

        return false;
    }

    public override void OnStateChange()
    {
        switch (lightLevel)
        {
            case LightLevel.Bright:
                brightTorches.Remove(this);
                lightLevel = LightLevel.Dim;
                dimTorches.Add(this);
                break;
            case LightLevel.Dim:
                dimTorches.Remove(this);
                lightLevel = LightLevel.Bright;
                brightTorches.Add(this);
                break;
        }
        UpdateVisuals();
    }

    public void Extinguish()
    {
        switch (lightLevel)
        {
            case LightLevel.Bright:
                brightTorches.Remove(this);
                break;
            case LightLevel.Dim:
                dimTorches.Remove(this);
                break;
        }

        lightLevel = LightLevel.Extinguished;
        UpdateVisuals();
        onExtinguish?.Invoke();
    }

    private void UpdateVisuals() => spriteRenderer.sprite = sprites[(int)lightLevel];
}
