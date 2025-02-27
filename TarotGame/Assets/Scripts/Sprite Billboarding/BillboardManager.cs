using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardManager : MonoBehaviour
{
    public static BillboardManager Instance;

    public void Init()
    {
        Instance = this;
    }

    private void Awake()
    {
        Init();
        UpdateSprites(Camera.main);
    }

    public void UpdateSprites(Camera cam)
    {
        foreach(var sprite in FindObjectsOfType<BillboardSprite>())
        {
            sprite.transform.rotation = cam.transform.rotation;
        }
    }
}
