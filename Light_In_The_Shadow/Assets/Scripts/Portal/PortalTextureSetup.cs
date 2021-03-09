using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PortalTextureSetup : MonoBehaviour {
    [SerializeField] private Camera [] _cameras;
    [SerializeField] private Material [] _materials;

    void Start() {
        for (int i = 0; i < _cameras.Length; i++) {
            SetCameraTexture(_cameras[i], _materials[i]);
        }
    }

    private void SetCameraTexture(Camera camera, Material material) {
        if (camera.targetTexture != null) camera.targetTexture.Release();
        camera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        material.mainTexture = camera.targetTexture;
    }
}