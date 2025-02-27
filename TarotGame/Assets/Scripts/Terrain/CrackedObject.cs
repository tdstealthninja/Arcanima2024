using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CrackedObject : MonoBehaviour
{
    public GridCollider gridCollider;
    public GridTransform playerGridTransform;
    public GridTransform gridTransform;
    public SpriteRenderer spriteRenderer;
    public Sprite collapsed;
    public UnityEvent onCollapse;

    private Vector3Int lastPlayerPos;

    private void Awake()
    {
        gridCollider = GetComponent<GridCollider>();
        gridTransform = GetComponent<GridTransform>();
        FindObjectOfType<PlayLoadedLevelV2>().onLoadEndedLate.RemoveListener(SetPlayerGridTransform);
        FindObjectOfType<PlayLoadedLevelV2>().onLoadEndedLate.AddListener(SetPlayerGridTransform);
        Debug.Log(FindObjectOfType<PlayLoadedLevelV2>());
    }

    public void OnCollapse()
    {
        gridCollider.CollidesWithMe = ~GridCollider.CollisionFlags.None;
        spriteRenderer.sprite = collapsed;
        Debug.Log("Collapse");
        onCollapse?.Invoke();
    }

    public void SetPlayerGridTransform()
    {
        playerGridTransform = FindObjectOfType<PlayerMovement>().GetComponent<GridTransform>();
        lastPlayerPos = playerGridTransform.GetGridPosition();
        Debug.Log("Set: ",playerGridTransform);
        playerGridTransform.onMoveEnd.RemoveListener(SavePrevPlayerPos);
        playerGridTransform.onMoveEnd.AddListener(SavePrevPlayerPos);
        playerGridTransform.onMoveStart.RemoveListener(CheckForCollapse);
        playerGridTransform.onMoveStart.AddListener(CheckForCollapse);
    }

    public void SavePrevPlayerPos()
    {
        lastPlayerPos = playerGridTransform.GetGridPosition();
    }

    public void CheckForCollapse()
    {
        if (lastPlayerPos == gridTransform.GetGridPosition())
        {
            OnCollapse();
        }
    }
}
