using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WorldSpaceTransitions;

public class ChangeFadeGizmo : MonoBehaviour
{
    public GameObject gizmo;

    private void Start()
    {
        gizmo = GizmoFollow.Instance.gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            gizmo.SetActive(true);
            gizmo.transform.position = transform.position;
        }
    }  
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            gizmo.SetActive(false);
        }
    }
}
