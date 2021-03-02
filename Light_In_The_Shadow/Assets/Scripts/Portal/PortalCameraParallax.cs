using UnityEngine;

public class PortalCameraParallax : MonoBehaviour {
    public Camera portalCamera;
    public Transform pairPortal;
    private void UpdateCamera(Camera camera) {
        portalCamera.projectionMatrix = camera.projectionMatrix;
        var relativePos = transform.InverseTransformPoint(camera.transform.position);
        relativePos = Vector3.Scale(relativePos, new Vector3(-1, -1, -1));
        portalCamera.transform.position = pairPortal.TransformDirection(relativePos);

        var relativeRot = transform.InverseTransformDirection(camera.transform.forward);
        relativeRot = Vector3.Scale(relativeRot, new Vector3(-1, -1, -1));
        portalCamera.transform.forward = pairPortal.TransformDirection(relativeRot);
    }
}