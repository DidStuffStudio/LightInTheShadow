using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditScroll : MonoBehaviour
{
    public float speed = 0.1f;

    private void Update()
    {
        transform.localPosition += new Vector3(0,-speed,0);
    }
}
