using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderOrder : MonoBehaviour
{
    private RenderOrderObject[] renderers;

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 playerPos = FindObjectOfType<PlayerMovement>().transform.position;

        Vector3 targetPos;

        if (renderers != null)
            for (int i = 0; i < renderers.Length; i++)
            {
                if (renderers[i] != null)
                {
                    targetPos = renderers[i].transform.position;
                    float dist = Vector3.Cross(Camera.main.transform.forward, targetPos - playerPos).magnitude;
                    renderers[i].SetRenderOrder((int)dist * 100);
                }
            }
    }

    public void UpdateLists()
    {
        RenderOrderObject[] sr = FindObjectsOfType<RenderOrderObject>();

        renderers = sr;
    }
}
