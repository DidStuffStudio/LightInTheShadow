using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTextureSetup : MonoBehaviour {
    public Camera cameraA;
    public Camera cameraB;

    public Material cameraMatA;
    public Material cameraMatB;

    void Start() {
        SetCameraTexture(cameraA, cameraMatA);
        SetCameraTexture(cameraB, cameraMatB);
    }

    private void SetCameraTexture(Camera camera, Material material) {
        if (camera.targetTexture != null) camera.targetTexture.Release();
        camera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        material.mainTexture = camera.targetTexture;
    }
}