using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCamera : MonoBehaviour {

	public Transform playerCamera;
	public Transform portal;
	public Transform otherPortal;
	
	// Update is called once per frame
	void Update () {
		

		float angularDifferenceBetweenPortalRotations = Quaternion.Angle(portal.rotation, otherPortal.rotation);
		Quaternion portalRotationalDifference = Quaternion.AngleAxis(angularDifferenceBetweenPortalRotations, Vector3.up);
		Vector3 newCameraDirection = portalRotationalDifference * playerCamera.forward;
		transform.localRotation = Quaternion.LookRotation(newCameraDirection, Vector3.up);
		
		// this works
		// transform.localRotation = Quaternion.LookRotation(angularDifferenceBetweenPortalRotations * otherPortal.InverseTransformDirection(otherPortal.forward), otherPortal.InverseTransformDirection(otherPortal.up));
		
		// this works better
		// transform.rotation = Quaternion.LookRotation(angularDifferenceBetweenPortalRotations * portal.forward, portal.up);
		
		// position of this portal camera
		Vector3 playerOffsetFromPortal = playerCamera.position - otherPortal.position;
		transform.position = portal.position + playerOffsetFromPortal;
	}
}
