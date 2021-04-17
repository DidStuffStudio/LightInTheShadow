using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowbarCollider : MonoBehaviour
{
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out WoodenPlank woodenPlank))
        {
           
            woodenPlank.Detach(other.ClosestPoint(transform.position));
        }
    }
}
