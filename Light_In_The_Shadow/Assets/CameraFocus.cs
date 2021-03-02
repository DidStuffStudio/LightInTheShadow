using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


public class CameraFocus : MonoBehaviour
{
    private Camera cam;
    private float cameraHeight;
    private FirstPersonLook fpsLook;
    private FirstPersonMovement fpsMove;
    private bool focused, focusChange = false;
    private Transform focalPoint;
    private GameObject lastHitObject;

    public float focusSpeed = 2.0f;
    public float interactionDistance = 5.0f;
    public GameObject canvas;

    void Start()
    {
        cam = Camera.main;
        cameraHeight = cam.gameObject.transform.localPosition.y;
        fpsLook = cam.gameObject.GetComponent<FirstPersonLook>();
        fpsMove = GetComponent<FirstPersonMovement>();
    }

    void Update()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;

        if (!focusChange && ! focused)
        {
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.layer == 9 && hit.distance < interactionDistance)
                {
                    hit.transform.GetComponent<Outline>().enabled = true;
                    if (Input.GetMouseButtonDown(0))
                    {
                        lastHitObject = hit.transform.gameObject;
                        hit.transform.GetComponent<Outline>().enabled = false;
                        focalPoint = hit.transform.Find("FocalPoint");
                        focusChange = true;
                    }
                }

                else
                {
                    lastHitObject.transform.GetComponent<Outline>().enabled = false;
                }
            }
        }

        if (Input.GetMouseButtonDown(1) && focused)
        {
            focusChange = true;
        }

        if (!focused && focusChange)
        {
            Focus(focalPoint);
        }

        if (focused && focusChange)
        {
            Defocus();
        }
    }

    void Focus(Transform focalPoint)
    {
        focusChange = true;
        cam.transform.parent = null;
        //cam.transform.position = focalPoint.position;
        //cam.transform.rotation = focalPoint.rotation;

        fpsLook.enabled = false;
        fpsMove.enabled = false;
        canvas.SetActive(false);
        cam.transform.position = Vector3.Lerp(cam.transform.position, focalPoint.position, Time.deltaTime * focusSpeed);
        cam.transform.rotation =
            Quaternion.Lerp(cam.transform.rotation, focalPoint.rotation, Time.deltaTime * focusSpeed);

        if (!(Vector3.Distance(cam.transform.position, focalPoint.position) < 0.1f) ||
            !(Quaternion.Angle(cam.transform.rotation, focalPoint.rotation) < 0.1f)) return;
        focused = true;
        focusChange = false;
        print("focused");
    }

    void Defocus()
    {
        cam.transform.parent = transform;
        //cam.transform.localPosition = new Vector3(0,cameraHeight,0);
        //cam.transform.localRotation = quaternion.identity;

        cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, new Vector3(0, cameraHeight, 0),
            Time.deltaTime * focusSpeed);
        cam.transform.localRotation =
            Quaternion.Lerp(cam.transform.localRotation, quaternion.identity, Time.deltaTime * focusSpeed);
        if (!(Vector3.Distance(cam.transform.localPosition, new Vector3(0, cameraHeight, 0)) < 0.1f) ||
            !(Quaternion.Angle(cam.transform.localRotation, quaternion.identity) < 0.1f)) return;
        print("exited");
        focused = false;
        focusChange = false;
        fpsLook.enabled = true;
        fpsMove.enabled = true;
        canvas.SetActive(true);
    }
}