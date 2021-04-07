using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingCollisionCheck : MonoBehaviour
{
    private FlyingBeanSpider _flyingBeanSpider;

    private void Start()
    {
        GetComponentInParent<FlyingBeanSpider>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == 10) return;
        _flyingBeanSpider.GetRandomPointInRange();
    }

}
