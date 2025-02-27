using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoonCard : MonoBehaviour
{
    [SerializeField]
    private GameObject shadowPrefab;

    private GridTransform gt;
    private GridCollider gc;
    private GameObject shadowButton;

    public UnityEvent onCollected;

    private void Awake()
    {
        shadowButton = Resources.FindObjectsOfTypeAll<ToggleShadow>()[0].gameObject;
        gt = GetComponent<GridTransform>();
        gc = GetComponent<GridCollider>();
    }

    private void Start()
    {
        GridTransform player = FindObjectOfType<PlayerMovement>().GetComponent<GridTransform>();
        player.onMoveEnd.RemoveListener(UpdateCard);
        player.onMoveEnd.AddListener(UpdateCard);
    }

    private void UpdateCard()
    {
        if (gc.CheckForCollision(gt.GetGridPosition()) != GridCollider.CollisionType.None)
        {
            onCollected?.Invoke();
            PlayerMovement player = FindObjectOfType<PlayerMovement>();
            GameObject shadow = Instantiate(shadowPrefab);
            player.UpdateShadow();
            shadow.GetComponent<GridTransform>().SetGridPosition(gt.GetGridPosition());
            shadowButton.SetActive(true);
            BillboardManager.Instance.UpdateSprites(Camera.main);
            player.GetComponent<GridTransform>().onMoveEnd.RemoveListener(UpdateCard);
            Destroy(gameObject);
        }
    }
}
