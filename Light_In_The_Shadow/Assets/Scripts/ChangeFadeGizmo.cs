using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFadeGizmo : MonoBehaviour
{
    public GameObject gizmo;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            //gizmo.transform.position = transform.position;
        }
    }
}
