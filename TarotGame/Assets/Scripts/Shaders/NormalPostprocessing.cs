using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalPostprocessing : MonoBehaviour
{
    public Material mat;
    private Camera cam;
    private void Awake()
    {
        cam = Camera.main;
        cam.targetTexture = null;
        cam.depthTextureMode = cam.depthTextureMode | DepthTextureMode.DepthNormals;
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Matrix4x4 viewToWorld = cam.cameraToWorldMatrix;
        mat.SetMatrix("_viewToWorld", viewToWorld);
        Graphics.Blit(source, destination, mat, -1);
    }
}
