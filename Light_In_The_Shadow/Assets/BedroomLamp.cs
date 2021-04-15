using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedroomLamp : MonoBehaviour
{
    public GameObject[] children;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Torch"))
        {
            foreach (var child in children)
            {
                child.SetActive(true);
            }
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Torch"))
        {
            foreach (var child in children)
            {
                child.SetActive(false);
            }
        }
    }
}
