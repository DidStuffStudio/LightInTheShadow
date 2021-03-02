using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour {
    private Camera _camera;
    public bool canInteract = true;

    private void Start() {
        _camera = Camera.main;
    }

    private void Update() {
        if (!canInteract) return;
        RaycastHit hit;
        // send a ray from the middle of the screen
        Ray ray = _camera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
            Debug.Log(hit.collider.name);
        }
    }
}