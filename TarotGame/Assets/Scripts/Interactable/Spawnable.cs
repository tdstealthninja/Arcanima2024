using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spawnable : MonoBehaviour
{
    [SerializeField]
    private GameObject toSpawn;

    private GridCollider gc;
    private GridTransform gt;

    private bool currentlySteppedOn = false;
    private bool steppedOnLastFrame = false;

    // Start is called before the first frame update
    void Start()
    {
        gc = GetComponent<GridCollider>();
        gt = GetComponent<GridTransform>();
        GridTransform player = FindObjectOfType<PlayerMovement>().GetComponent<GridTransform>();
        player.onMoveEnd.RemoveListener(UpdateSpawnable);
        player.onMoveEnd.AddListener(UpdateSpawnable);
    }

    // Update is called once per frame
    private void UpdateSpawnable()
    {
        steppedOnLastFrame = currentlySteppedOn;

        if (gc.CheckForCollision(gt.GetGridPosition()) != GridCollider.CollisionType.None)
        {
            currentlySteppedOn = true;
        }
        else
        {
            currentlySteppedOn = false;

            if (steppedOnLastFrame)
            {
                GameObject go = Instantiate(toSpawn);
                Vector3Int pos = gt.GetGridPosition();
                Destroy(gc);
                Destroy(gt);
                go.GetComponent<GridTransform>().SetGridPosition(pos, 0);
                BillboardManager.Instance.UpdateSprites(Camera.main);
                FindObjectOfType<PlayerMovement>().GetComponent<GridTransform>().onMoveEnd.RemoveListener(UpdateSpawnable);
                Destroy(gameObject);
            }
        }
    }
}
