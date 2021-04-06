using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialNumber : MonoBehaviour {
    [SerializeField] private PhonePuzzle phonePuzzle;
    private void OnMouseDown() {
        phonePuzzle.currentNumber = this.gameObject.name;
        phonePuzzle._isRotating = true;
        phonePuzzle.mouseDownPosition = Input.mousePosition;
    }

    private void OnMouseUp() {
        phonePuzzle._isRotating = false;
        phonePuzzle.UpdateDialNumber();
        phonePuzzle.goingBackToOriginalPos = true;
    }
    
}
