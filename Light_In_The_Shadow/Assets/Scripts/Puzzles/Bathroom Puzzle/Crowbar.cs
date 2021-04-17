using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crowbar : MonoBehaviour {
    [SerializeField] private BathroomPuzzle bathroomPuzzle;
    public bool pullCrowbarUpWardsToDetach;
    public Quaternion originalRot;
    public bool atOriginalRot;

    private void Start()
    {
        originalRot = transform.rotation;
    }

    private void OnMouseDown() {
        bathroomPuzzle._isRotating = true;
        bathroomPuzzle.mouseDownPosition = Input.mousePosition;
    }

    private void OnMouseUp() {
        bathroomPuzzle._isRotating = false;
        bathroomPuzzle.goingBackToOriginalPos = true;
    }

    private void Update()
    {
        atOriginalRot = Quaternion.Angle(transform.rotation, originalRot) < 1.0f;
    }
}