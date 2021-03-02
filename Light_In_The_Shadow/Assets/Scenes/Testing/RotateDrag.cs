using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateDrag : MonoBehaviour
{
    
     
    private float _sensitivity;
    private Vector3 _mouseReference;
    private Vector3 _mouseOffset;
    private Vector3 _rotation;
    private bool _isRotating;
    public GameObject antennaBase;
     
    void Start ()
    {
        _sensitivity = 0.4f;
        _rotation = Vector3.zero;
    }
     
    void Update()
    {
        if(_isRotating)
        {
            // offset
            _mouseOffset = (Input.mousePosition - _mouseReference);
            // apply rotation
            //_rotation.y = -(_mouseOffset.x + _mouseOffset.y) * _sensitivity;
            //_rotation.y = -(_mouseOffset.x) * _sensitivity;
            _rotation.x = -(_mouseOffset.x) * _sensitivity;
            // rotate
            //transform.Rotate(_rotation);
           // transform.eulerAngles += _rotation;

           if (transform.rotation.x >50 && transform.rotation.x<10)
           {
               transform.rotation.x = 25;
           }

           transform.RotateAround(antennaBase.transform.position,Vector3.right,_rotation.x);
            // store mouse
            _mouseReference = Input.mousePosition;
            Debug.Log(_rotation.x);
        }
    }
     
    void OnMouseDown()
    {
        // rotating flag
        _isRotating = true;
         
        // store mouse
        _mouseReference = Input.mousePosition;
    }
     
    void OnMouseUp()
    {
        // rotating flag
        _isRotating = false;
    }
     
}
