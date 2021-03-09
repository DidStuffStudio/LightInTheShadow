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
    public GameObject antenna;
    public GameObject screen;

    private float rotationAngle;
    private float quatRotation;
    private Vector3 _previousMouseOffset;
    Renderer rend;
    
     
    void Start ()
    {
        _sensitivity = 0.1f;
        _rotation = Vector3.zero;
        rend = screen.GetComponent<Renderer>();
        //MeshRenderer rend = screen.GetComponent<MeshRenderer>();
    }
     
    void Update()
    {
        if(_isRotating)
        {

            quatRotation = antenna.transform.rotation.eulerAngles.x;
            rotationAngle = antenna.transform.rotation.eulerAngles.x;
            Debug.Log("rotation " + quatRotation);
            
            _mouseOffset = (Input.mousePosition - _mouseReference);
            Debug.Log(_rotation.x);
            _rotation.x = -(_mouseOffset.x) * _sensitivity;
            
            if (quatRotation>85 && _mouseOffset.x <= _previousMouseOffset.x)
            {
                //_rotation.x += _rotation.x;
              
                
                
                return;
            }

            if (quatRotation<5 && _mouseOffset.x >= _previousMouseOffset.x)
            {
                return;
            }
            _previousMouseOffset = _mouseOffset;
            rotationAngle -= 15;

            rotationAngle=Map(rotationAngle, 0, 360, 0, 20f);
        
       
            //Debug.Log(screen.GetComponent<MeshRenderer>().material.GetFloat("Vector1_c2ed4abf816445a0a71419d361dcdacf")); 
            screen.GetComponent<MeshRenderer>().material.SetFloat("Vector1_c2ed4abf816445a0a71419d361dcdacf",rotationAngle);
            

            

            
            transform.RotateAround(antennaBase.transform.position,Vector3.right,_rotation.x);
            
            // store mouse
            _mouseReference = Input.mousePosition;


        }
        


        //rend.material.SetFloat("_TVTransition",rotationAngle);
        //Debug.Log(rend.material.GetFloat("_TVTransition"));
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
    
    private float Map(float value, float min1, float max1, float min2, float max2) {

        float r = min2 + (max2 - min2) * ((value - min1) / (max1-min1));

        return r;

    }
     
}
