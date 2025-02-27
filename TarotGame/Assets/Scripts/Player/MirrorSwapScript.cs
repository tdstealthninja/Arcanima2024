using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MirrorSwapScript : MonoBehaviour
{
    private GridTransform gridTransform;
    //[SerializeField]
    //private float distanceToPush = 1;
    //[SerializeField]
    //private bool locked = false;
    //private bool pushed = false;
    private Shadow shadowExists;
    private PlayerMovement player;
    private GridTransform playerPos;
    private GridTransform shadowPos;

    public UnityEvent onSwap;

    // Start is called before the first frame update
    void Start()
    {
        PlayLoadedLevelV2 levelPlayer = FindObjectOfType<PlayLoadedLevelV2>();
        levelPlayer.onLoadEnded.RemoveListener(OnLevelLoaded);
        levelPlayer.onLoadEnded.AddListener(OnLevelLoaded);
    }

    private void OnLevelLoaded()
    {
        gridTransform = GetComponent<GridTransform>();
    }

    public bool Swap()
    {
        shadowExists = FindObjectOfType<Shadow>();
        player = FindObjectOfType<PlayerMovement>();
        //Debug.Log("Interacted with mirror");

        if (shadowExists == null || !shadowExists.isActiveAndEnabled)
        {
            //Debug.Log("Shadow not active");
            return false;
        }

        else
        {
            //Debug.Log("Shadow exists");
            shadowPos = shadowExists.gameObject.GetComponent<GridTransform>();
            playerPos = player.GetComponent<GridTransform>();
            Vector3Int shadowtmpPos = shadowPos.GetGridPosition();
            Vector3Int playertmpPos = playerPos.GetGridPosition();
            if (!shadowtmpPos.Equals(playertmpPos))
            {
                //Debug.Log("Swapping player and Shadow");
                player.SwapPosition(shadowtmpPos);
                shadowExists.SwapPosition(playertmpPos);
                return true;
            }
            return false;
        }

    }
}
