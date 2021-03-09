using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    public Camera cam;
    public float interactionDistance;
    private GameObject lastHitObject;
    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.layer == 11 && hit.distance < interactionDistance)
            {
                hit.transform.GetComponent<Outline>().enabled = true;
                if (Input.GetMouseButtonDown(0))
                {
                    if (hit.transform.gameObject.GetComponent<DetectClick>())
                    {
                        hit.transform.gameObject.GetComponent<DetectClick>().Click();
                    }
                }
            }
            

            else
            {
                if (lastHitObject.transform.gameObject.GetComponent<Outline>())
                {
                    lastHitObject.transform.GetComponent<Outline>().enabled = false;
                }

            }
        }

    }
}
