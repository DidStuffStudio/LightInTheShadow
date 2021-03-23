using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Portal : MonoBehaviour {
    public Portal linkedPortal;
    public Camera portalCam;
    private CinemachineVirtualCamera _playerCamera;

    void Awake () {
        _playerCamera = FindObjectOfType<CinemachineVirtualCamera>();
    }

    private void Update() {
        UpdatePortalCamera();
    }

    private void UpdatePortalCamera() {
        // make portal camera position and rotation the same relative to this portal as player camera relative to linked portal
        var m = transform.localToWorldMatrix * linkedPortal.transform.worldToLocalMatrix *
                _playerCamera.transform.localToWorldMatrix;
        portalCam.transform.SetPositionAndRotation(m.GetColumn(3), m.rotation);
    }
}