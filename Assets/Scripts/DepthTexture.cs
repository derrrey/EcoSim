﻿using UnityEngine;

[ExecuteInEditMode]
public class DepthTexture : MonoBehaviour
{
    Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
        cam.depthTextureMode = DepthTextureMode.Depth;
    }
}