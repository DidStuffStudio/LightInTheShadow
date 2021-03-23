using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateObjectInRange : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 14)
        {
            other.GetComponent<MeshRenderer>().enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 14)
        {
            other.GetComponent<MeshRenderer>().enabled = false;
        }
        
    }
}
