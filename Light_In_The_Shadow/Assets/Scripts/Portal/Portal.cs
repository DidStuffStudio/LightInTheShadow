using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {
    public Portal linkedPortal;
    // public MeshRenderer screen;
    
    [SerializeField] private Camera portalCam;
    private Camera playerCam;

    void Awake () {
        playerCam = Camera.main;
    }

    private void Update() {
        UpdatePortalCamera();
    }

    private void UpdatePortalCamera() {
        /*
        var localToWorldMatrix = playerCam.transform.localToWorldMatrix;

        portalCam.projectionMatrix = playerCam.projectionMatrix;
        localToWorldMatrix = transform.localToWorldMatrix * linkedPortal.transform.worldToLocalMatrix * localToWorldMatrix;
        portalCam.transform.SetPositionAndRotation (localToWorldMatrix.GetColumn(3), localToWorldMatrix.rotation);
*/
        // make portal camera position and rotation the same relative to this portal as player camera relative to linked portal
        var m = transform.localToWorldMatrix * linkedPortal.transform.worldToLocalMatrix *
                playerCam.transform.localToWorldMatrix;
        portalCam.transform.SetPositionAndRotation(m.GetColumn(3), m.rotation);

    }
}