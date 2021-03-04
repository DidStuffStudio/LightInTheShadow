using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RenderPipeline = UnityEngine.Rendering.RenderPipeline;
[ExecuteInEditMode]
public class PortalNew : MonoBehaviour
{
    public Camera portalCam;
    public Transform pairPortal;


    // Update is called once per frame
    void UpdateCamera(Camera camera)
    {
        if ((camera.cameraType == CameraType.Game || camera.cameraType == CameraType.SceneView) && camera.tag != "Portal Camera")
        {
            portalCam.projectionMatrix = camera.projectionMatrix;
            var relativePosition = transform.InverseTransformPoint(camera.transform.position);
            relativePosition = Vector3.Scale(relativePosition, new Vector3(-1,1,-1));
            portalCam.transform.position = pairPortal.TransformPoint(relativePosition);
            
            var relativeRotation = transform.InverseTransformDirection(camera.transform.forward);
            relativeRotation = Vector3.Scale(relativeRotation, new Vector3(-1,1,-1));
            portalCam.transform.forward = pairPortal.TransformDirection(relativeRotation);
        }
    }
}
