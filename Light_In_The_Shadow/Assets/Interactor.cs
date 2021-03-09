using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    public Camera cam;
    public float interactionDistance;
    private GameObject _lastHitObject;
    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit)) return;
        if (hit.transform.gameObject.layer == 11 && hit.distance < interactionDistance)
        {
            _lastHitObject = hit.transform.gameObject;
            hit.transform.GetComponent<Outline>().enabled = true;
            if (!Input.GetMouseButtonDown(0)) return;
            if (hit.transform.gameObject.GetComponent<DetectClick>() == null) return;
            hit.transform.gameObject.GetComponent<DetectClick>().Click();
        }
        
        else
        {
            if (_lastHitObject == null) return;
            if (_lastHitObject.transform.gameObject.GetComponent<Outline>() == null) return;
            _lastHitObject.transform.GetComponent<Outline>().enabled = false;
        }

    }
}
