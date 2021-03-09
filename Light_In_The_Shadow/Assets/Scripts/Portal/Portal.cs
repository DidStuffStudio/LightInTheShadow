using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {
    public Portal linkedPortal;
    [SerializeField] private Camera portalCam;
    private Camera playerCam;

    void Awake () {
        playerCam = Camera.main;
    }

    private void Update() {
        UpdatePortalCamera();
    }

    private void UpdatePortalCamera() {
        // make portal camera position and rotation the same relative to this portal as player camera relative to linked portal
        var m = transform.localToWorldMatrix * linkedPortal.transform.worldToLocalMatrix *
                playerCam.transform.localToWorldMatrix;
        portalCam.transform.SetPositionAndRotation(m.GetColumn(3), m.rotation);
    }
}