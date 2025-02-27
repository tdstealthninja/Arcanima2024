using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderOrderObject : MonoBehaviour
{
    public void SetRenderOrder(int order)
    {
        GetComponentInChildren<SpriteRenderer>().sortingOrder = order;
    }
}
