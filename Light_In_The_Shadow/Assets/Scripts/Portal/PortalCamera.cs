using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class PortalCamera : MonoBehaviour {
    [Header("Cameras")] [SerializeField] private Transform playerCamera;
    [SerializeField] private Camera targetPortalCamera;

    [Space] [Header("Materials")] [SerializeField]
    private Shader shader;

    private Material _material;

    [Space] [Header("Portals")] [SerializeField]
    private Transform portal;

    [SerializeField] private Transform otherPortal;

    [Space] 
    
    [Header("Render plane")] 
    [SerializeField]
    private GameObject renderPlane;

    private void Start() {
        playerCamera = Camera.main.transform;
        SetCameraTexture();
    }

    private void SetCameraTexture() {
        if (targetPortalCamera == null) throw new ArgumentNullException(nameof(targetPortalCamera));
        if (shader == null) throw new ArgumentNullException(nameof(shader));
        if (_material == null) {
            // throw new ArgumentNullException(nameof(material));
            _material = new Material(shader) {name = transform.name + "_Mat"};

            renderPlane.GetComponent<Renderer>().material = _material;
        }

        if (targetPortalCamera.targetTexture != null) targetPortalCamera.targetTexture.Release();
        targetPortalCamera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        _material.mainTexture = targetPortalCamera.targetTexture;
    }

    private void Update() {
        Vector3 playerOffsetFromPortal = playerCamera.position - otherPortal.position;
        transform.position = portal.position + playerOffsetFromPortal;

        float angularDifferenceBetweenPortalRotations = Quaternion.Angle(portal.rotation, otherPortal.rotation);

        Quaternion portalRotationalDifference =
            Quaternion.AngleAxis(angularDifferenceBetweenPortalRotations, Vector3.up);
        Vector3 newCameraDirection = portalRotationalDifference * playerCamera.forward;
        transform.rotation = Quaternion.LookRotation(newCameraDirection, Vector3.up);
    }
}