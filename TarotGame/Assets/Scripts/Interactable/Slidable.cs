using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slidable : Interactable
{
    [SerializeField]
    private bool willSlide = false;
    [SerializeField]
    private Sprite[] sprites;

    private SpriteRenderer spriteRenderer;
    private GridTransform gridTransform;
    private GridCollider gridCollider;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        gridTransform = GetComponent<GridTransform>();
        gridCollider = GetComponent<GridCollider>();

        FindObjectOfType<StateChange>().onStateChange.RemoveListener(OnStateChange);
        FindObjectOfType<StateChange>().onStateChange.AddListener(OnStateChange);

        UpdateVisuals();
    }

    public override bool Interact(Interactor interactor)
    {
        bool success = false;
        Vector3Int pushDirection = Vector3Int.RoundToInt(transform.position - interactor.transform.position);
        Vector3Int targetLocation = gridTransform.GetGridPosition() + pushDirection;
        Vector3Int testLocation = targetLocation;

        if (willSlide)
        {
            while(gridCollider.CheckForCollision(testLocation) == GridCollider.CollisionType.None)
            {
                targetLocation = testLocation;
                testLocation += pushDirection;
            }

        }

        float dist = (targetLocation - gridTransform.GetGridPosition()).magnitude;

        success = gridTransform.SetGridPosition(targetLocation, (willSlide ? 0.1f : gridTransform.Speed) * dist);

        if (success)
            onInteractionSuccess?.Invoke(interactor, this);

        return success;
    }

    public override void OnStateChange()
    {
        willSlide = !willSlide;

        UpdateVisuals();
    }

    private void UpdateVisuals() => spriteRenderer.sprite = sprites[willSlide ? 1 : 0];
}
