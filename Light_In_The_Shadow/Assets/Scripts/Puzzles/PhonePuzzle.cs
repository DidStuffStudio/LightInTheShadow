using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

public class PhonePuzzle : MonoBehaviour {

    private Camera _camera;
    
    // reference to the phone game object to rotate around
    [SerializeField] private GameObject rotaryPhone;
    [SerializeField] private float goBackRotationSpeed = 1;
    public bool _isRotating = false;
    public float _rotation;
    [SerializeField] private float rotationSensitivity = 0.1f;
    public Vector3 _mouseDownPosition;
    public bool goingBackToOriginalPos;
    
    
    public string currentNumber;
    private string [] possibleNumbersToCall = {"911", "112"};
    public string _dialedNumber = "";
    private Quaternion originalRotation;

    private void Start() {
        _camera = Camera.main;
        originalRotation = transform.rotation;
    }

    private void Update() {
        if (_isRotating) {
            _rotation = rotaryPhone.transform.rotation.eulerAngles.x;
            var mouseOffset = Input.mousePosition.x - _mouseDownPosition.x;
            // var rotationValue = (mouseOffset > 0) ? -Input.mousePosition.x - _mouseDownPosition.x : Input.mousePosition.x - _mouseDownPosition.x;
            _rotation = -Mathf.Abs(mouseOffset * rotationSensitivity);
            // print(_rotation);
            transform.RotateAround(rotaryPhone.transform.position, Vector3.forward, angle: _rotation);
        }
        else {
            transform.rotation = Quaternion.Lerp(transform.rotation, originalRotation, goBackRotationSpeed);
        }
    }

    private void GetDialNumber() {
        if(_isRotating) return;
        RaycastHit hit;
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
            if (hit.collider.CompareTag(GameConstantStrings.Tags.DialNumber)) {
                currentNumber = hit.collider.name;
                print(currentNumber);
            }
        }
    }


    public void UpdateDialNumber() {
        // check if any of the possible numbers to call contains the currently dialed number
        var aux = _dialedNumber + currentNumber;
        foreach (var number in possibleNumbersToCall) {
            if (number == aux) {
                // the user starts calling the possible number
                _dialedNumber = aux;
                CallNumber();
                break;
            }
            // if the already dialed number + this number is part of the possible numbers, add it to the dialed number
            if (number.StartsWith(aux)) {
                _dialedNumber = aux;
            }
        }
    }

    private void CallNumber() {
        print("Calling " + _dialedNumber);
        _dialedNumber = "";
    }
}