using System;
using UnityEngine;
using System.Collections;

public class csParticleMove : MonoBehaviour
{
    public float speed = 0.1f;
    private Rigidbody _rigidbody;

    private void Start()
    {
	    _rigidbody = GetComponent<Rigidbody>();
    }

    void Update () {
        transform.Translate(Vector3.back * speed);
        _rigidbody.AddForce(Vector3.forward*speed, ForceMode.Force);
	}
}
