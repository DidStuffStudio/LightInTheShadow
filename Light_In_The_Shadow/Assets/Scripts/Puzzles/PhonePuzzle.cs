using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Instrumentation;
using System.Runtime.CompilerServices;
using Puzzles;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

public class PhonePuzzle : PuzzleMaster
{
    [Space] [Header("Unique Parameters")] 
    [SerializeField] private GameObject numbersCanvas;

    [HideInInspector] 
    public bool goingBackToOriginalPos, _isRotating;
    [HideInInspector]
    public Vector3 mouseDownPosition = Vector3.zero;
    [HideInInspector]
    public string currentNumber;
    
    private readonly float _goBackRotationSpeed = 0.2f;
    private readonly float _rotationSensitivity = 0.01f;
    private readonly string [] _possibleNumbersToCall = {"911", "112", "999", "000"};
    private string _dialedNumber = "";
    private Quaternion _originalRotation;
    private float _rotation;
    protected override void Start() {
        base.Start();
        fadeSpeedIncrement = 0.001f;
        _originalRotation = puzzleObject.transform.rotation;
    }

    protected override void Update()
    { 
        base.Update();
     if (currentNumber.Length > 3) currentNumber = "";
        if (_isRotating) {
            _rotation = puzzleObject.transform.localRotation.eulerAngles.x;
            var mousePosition = Input.mousePosition - mouseDownPosition;
            _rotation = Mathf.Abs(Vector3.Magnitude(mousePosition) * _rotationSensitivity);
            puzzleObject.transform.RotateAround(puzzleObject.transform.position, puzzleObject.transform.forward, angle: _rotation);
        }
        else puzzleObject.transform.rotation = Quaternion.Lerp(puzzleObject.transform.rotation, _originalRotation, _goBackRotationSpeed);
    }
    
    public void UpdateDialNumber() {
        // check if any of the possible numbers to call contains the currently dialed number
        var aux = _dialedNumber + currentNumber;
        foreach (var number in _possibleNumbersToCall) {
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
        
        _dialedNumber = "";
        correct = true;
    }
    protected override void FadeOutCutscene()
    {
        numbersCanvas.SetActive(false);
        base.FadeOutCutscene();
    }

    protected override void FadeInScene()
    {
        numbersCanvas.SetActive(true);
        base.FadeInScene();
    }

    public void FocusOnPhone(bool focus) => base.FocusOnPuzzleItem(focus);
    public void EndKitchenCutscene() => base.EndCutScene();
    public void SwitchChildAnimation() => base.BoyAnimations();

}